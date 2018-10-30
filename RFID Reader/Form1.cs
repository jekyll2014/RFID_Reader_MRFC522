using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RFID_Reader
{
    public partial class Form1 : Form
    {
        private List<byte> cardData = new List<byte>();
        private int _codePage = RFID_Reader_MRFC522.Properties.Settings.Default.CodePage;
        private int _tmr_delay = RFID_Reader_MRFC522.Properties.Settings.Default.RefreshTimeout;

        public static System.Timers.Timer _aTimer;
        public MFRC522 reader = new MFRC522();
        private byte[] _uid_old = new byte[0];
        private byte _sak_old = 0;
        private byte _keySize = 6;
        private byte _pageSize = 16;
        private byte[] _defaultKeyA = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        private byte[] _defaultKeyB = null;
        private static Semaphore mutexObj = new Semaphore(0, 1);

        public Form1()
        {
            InitializeComponent();

            _aTimer = new System.Timers.Timer();
            _aTimer.Interval = _tmr_delay;
            _aTimer.Elapsed += RFID_get;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = false;

            button_refersh_Click(this, EventArgs.Empty);
        }

        private void RFID_get(object sender, EventArgs e)
        {
            mutexObj.WaitOne(_tmr_delay);
            _aTimer.Enabled = false;
            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Yellow;
                label_tagFound.Text = "Looking for tag";
            });
            byte[] uid_new = RFID_hunt();

            // No tag
            if (uid_new == null)
            {
                _uid_old = new byte[0];
                _sak_old = 0;
                this.Invoke((MethodInvoker)delegate
                {
                    label_tagFound.BackColor = Color.Red;
                    label_tagFound.Text = "Tag not found";
                    textBox_tagType.Text = "";
                    textBox_tagUUID.Text = "";
                });
            }
            // If we have the new tag
            else if (!Accessory.ByteArrayCompare(uid_new, _uid_old))
            {
                _uid_old = uid_new;
                // Select the scanned tag
                _sak_old = reader.uid.sak;
                // show collected data
                this.Invoke((MethodInvoker)delegate
                {
                    label_tagFound.BackColor = Color.Green;
                    label_tagFound.Text = "Tag found";
                    MFRC522.PICC_Type t = reader.PICC_GetType(reader.uid.sak);
                    textBox_tagType.Text = reader.PICC_GetTypeName(t);
                    textBox_tagUUID.Text = Accessory.ConvertByteArrayToHex(uid_new);
                });
                //reader.PCD_StopCrypto1();
            }
            // Old tag still present
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    label_tagFound.BackColor = Color.Green;
                    label_tagFound.Text = "Tag found";
                    MFRC522.PICC_Type t = reader.PICC_GetType(_sak_old);
                    textBox_tagType.Text = reader.PICC_GetTypeName(t);
                    textBox_tagUUID.Text = Accessory.ConvertByteArrayToHex(_uid_old);
                });
            }
            _aTimer.Enabled = true;
            mutexObj.Release();
        }

        private byte[] RFID_hunt()
        {
            bool status = false;
            int tryGetCard = 3;
            while (tryGetCard > 0)
            {
                // Scan for cards
                if (reader.PICC_IsAnyCardPresent())
                {
                    status = reader.PICC_ReadCardSerial();
                    if (status)
                    {
                        break;
                    }
                }
                tryGetCard--;
            }
            // if no card in 3 retries
            if (tryGetCard == 0 && !status)
            {
                //button_Start_Click(this, EventArgs.Empty);
                return null;
            }
            // If we have the UID
            else if (reader.uid.size > 0)
            {
                byte[] ret = new byte[reader.uid.size];
                for (int i = 0; i < reader.uid.size; i++) ret[i] = reader.uid.uidByte[i];
                return reader.uid.uidByte;
            }
            return null;
        }

        private List<byte[]> RFID_read(byte[] uid, byte[] keyA, byte[] keyB, byte sectorStart, byte sectorEnd)
        {
            if (uid != null && uid.Length > 1)
            {
                List<byte[]> data = new List<byte[]>();
                byte sectorUL = 0;
                for (byte sector = sectorStart; sector <= sectorEnd; sector++)
                {
                    MFRC522.StatusCode status = new MFRC522.StatusCode();
                    MFRC522.PICC_Type t = reader.PICC_GetType(reader.uid.sak);
                    if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_MINI || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_1K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_4K)
                    {
                        // This is the default key for authentication
                        byte[] key = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                        if (keyA != null && keyA.Length != 6)
                        {
                            keyA = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        }
                        if (keyB != null && keyB.Length != 6)
                        {
                            keyB = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        }
                        byte[] tmp_uid = RFID_hunt();
                        if (!Accessory.ByteArrayCompare(tmp_uid, uid)) data.Add(null);
                        // Authenticate
                        if (keyA != null) status = reader.PCD_Authenticate((byte)MFRC522.PICC_Command.PICC_CMD_MF_AUTH_KEY_A, sector, keyA, reader.uid);
                        if (keyB != null) status = reader.PCD_Authenticate((byte)MFRC522.PICC_Command.PICC_CMD_MF_AUTH_KEY_B, sector, keyB, reader.uid);

                        // Check if authenticated
                        if (status == MFRC522.StatusCode.STATUS_OK)
                        {
                            byte[] buffer = new byte[18];
                            byte byteCount = (byte)buffer.Length;
                            status = reader.MIFARE_Read(sector, out buffer, byteCount);
                            if (status != MFRC522.StatusCode.STATUS_OK)
                            {
                                data.Add(null);
                            }
                            else
                            {

                                byte[] tmp = new byte[buffer.Length - 2];
                                for (int i = 0; i < buffer.Length - 2; i++)
                                {
                                    tmp[i] = buffer[i];
                                }
                                data.Add(tmp);
                            }
                        }
                        else
                        {
                            data.Add(null);
                        }
                        reader.PCD_StopCrypto1();
                    }
                    else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_UL)
                    {
                        byte[] buffer = new byte[18];
                        byte byteCount = (byte)buffer.Length;
                        status = reader.MIFARE_Read(sectorUL, out buffer, byteCount);
                        if (status != MFRC522.StatusCode.STATUS_OK)
                        {
                            data.Add(null);
                        }
                        else
                        {
                            byte[] tmp = new byte[buffer.Length - 2];
                            for (int i = 0; i < buffer.Length - 2; i++)
                            {
                                tmp[i] = buffer[i];
                            }
                            data.Add(tmp);

                        }
                        sectorUL += 4;
                    }
                }
                return data;
            }
            return null;
        }

        private bool RFID_write(byte[] uid, byte[] keyA, byte[] keyB, byte sector, byte[] data)
        {
            if (uid != null && uid.Length > 1 && data.Length >= 16)
            {
                // This is the default key for authentication
                //byte[] key = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                if (keyA != null && keyA.Length != 6)
                {
                    keyA = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                }
                if (keyB != null && keyB.Length != 6)
                {
                    keyB = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                }

                MFRC522.StatusCode status = new MFRC522.StatusCode();
                MFRC522.PICC_Type t = reader.PICC_GetType(reader.uid.sak);
                if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_MINI || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_1K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_4K)
                {
                    // Authenticate
                    if (keyA != null) status = reader.PCD_Authenticate((byte)MFRC522.PICC_Command.PICC_CMD_MF_AUTH_KEY_A, sector, keyA, reader.uid);
                    if (keyB != null) status = reader.PCD_Authenticate((byte)MFRC522.PICC_Command.PICC_CMD_MF_AUTH_KEY_B, sector, keyB, reader.uid);

                    // Check if authenticated
                    if (status == MFRC522.StatusCode.STATUS_OK)
                    {
                        status = reader.MIFARE_Write(sector, data, (byte)data.Length);
                    }
                    else
                    {
                        return false;
                    }
                    reader.PCD_StopCrypto1();
                }
                else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_UL)
                {
                    /*byte[] PSWBuff = new byte[4] { 0xff, 0xff, 0xff, 0xff };
                    byte[] pACK = new byte[2];
                    status = reader.PCD_NTAG216_AUTH(PSWBuff, out pACK);
                    if (status == MFRC522.StatusCode.STATUS_OK)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            byte[] tmp = new byte[4] { data[0 + i * 4], data[1 + i * 4], data[2 + i * 4], data[3 + i * 4] };
                            status = reader.MIFARE_Ultralight_Write((byte)(sector * 4 + i), tmp, (byte)tmp.Length);
                        }
                    }*/
                    MessageBox.Show("Can only write to MIFARE CLASSIC MINI/1K/4K");
                }
                return (status == MFRC522.StatusCode.STATUS_OK);
            }
            return false;
        }

        private void button_refersh_Click(object sender, EventArgs e)
        {
            comboBox_portName.Items.Clear();
            comboBox_portName.Items.Add("None");
            comboBox_portName.Items.AddRange(MFRC522.getComPorts()); //добавить порты в список
            if (comboBox_portName.Items.Count == 1)
            {
                comboBox_portName.SelectedIndex = 0;
                button_start.Enabled = false;
            }
            else
            {
                comboBox_portName.SelectedIndex = 1;
            }
            comboBox_speed.SelectedItem = "9600";
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            mutexObj.WaitOne(_tmr_delay);
            if (button_start.Text == "Start reading")
            {
                if (comboBox_portName.SelectedIndex == 0)
                {
                    mutexObj.Release();
                    return;
                }
                reader.Open(comboBox_portName.Text, int.Parse(comboBox_speed.Text));
                if (reader.IsConnected)
                {
                    reader.PCD_Init();
                    button_start.Text = "Stop reading";
                    label_tagFound.BackColor = Color.Red;
                    button_read.Enabled = true;
                    button_write.Enabled = true;
                    button_clear.Enabled = true;
                    comboBox_portName.Enabled = false;
                    comboBox_speed.Enabled = false;
                    button_refersh.Enabled = false;
                    _aTimer.Enabled = true;
                    this.Refresh();
                }
            }
            else
            {
                _aTimer.Enabled = false;
                //Accessory.Delay_ms(_tmr_delay + 100);
                button_start.Text = "Start reading";
                label_tagFound.BackColor = Color.Gray;
                label_tagFound.Text = "";
                reader.Close();
                button_read.Enabled = false;
                button_write.Enabled = false;
                button_clear.Enabled = false;
                comboBox_portName.Enabled = true;
                comboBox_speed.Enabled = true;
                button_refersh.Enabled = true;
            }
            mutexObj.Release();
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            mutexObj.WaitOne(_tmr_delay);
            _aTimer.Enabled = false;
            //Accessory.Delay_ms(_tmr_delay + 100);

            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Yellow;
                label_tagFound.Text = "Reading tag";
                this.Refresh();
            });
            checkBox_tagHex.Checked = true;

            for (byte i = 0; i <= 8; i++)
            {
                Control[] controls = Controls.Find("textBox_tagEdit" + i.ToString(), true);
                if (controls.Length > 0)
                {
                    TextBox tBox = controls[0] as TextBox;
                    tBox.Text = "";
                    tBox.BackColor = Color.White;
                }
            }

            List<byte[]> data = new List<byte[]>();
            byte[] uid = RFID_hunt();
            if (uid != null)
            {
                byte num = 8;
                data = RFID_read(uid, _defaultKeyA, _defaultKeyB, 0, (byte)(num - 1));
                if (data.Count == num)
                {
                    for (byte i = 0; i < num; i++)
                    {
                        Control[] controls = Controls.Find("textBox_tagEdit" + i.ToString(), true);
                        if (controls.Length > 0)
                        {
                            TextBox tBox = controls[0] as TextBox;
                            if (data[i] != null)
                            {
                                tBox.Text = Accessory.ConvertByteArrayToHex(data[i]);
                            }
                            else tBox.Text = "can't read";
                        }
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        label_tagFound.BackColor = Color.Green;
                        label_tagFound.Text = "Tag read";
                    });
                }
            }
            else
            {
                textBox_tagEdit0.Text = "Can't find tag";
            }

            _aTimer.Enabled = true;
            mutexObj.Release();
        }

        private void button_write_Click(object sender, EventArgs e)
        {
            mutexObj.WaitOne(_tmr_delay);
            _aTimer.Enabled = false;
            //Accessory.Delay_ms(_tmr_delay + 100);

            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Yellow;
                label_tagFound.Text = "Writing tag";
                this.Refresh();
            });
            checkBox_tagHex.Checked = true;

            for (byte i = 1; i <= 8; i++)
            {
                Control[] controls = Controls.Find("checkBox_page" + i.ToString(), true);
                if (controls.Length > 0)
                {
                    CheckBox cBox = controls[0] as CheckBox;

                    if (cBox.Checked)
                    {
                        controls = Controls.Find("textBox_tagEdit" + i.ToString(), true);
                        if (controls.Length > 0)
                        {
                            TextBox tBox = controls[0] as TextBox;
                            byte[] data = Accessory.ConvertHexToByteArray(tBox.Text);
                            if (data.Length == 16)
                            {
                                byte[] uid = null;
                                uid = RFID_hunt();
                                if (uid != null)
                                {
                                    bool status = RFID_write(uid, _defaultKeyA, _defaultKeyB, i, data);
                                    if (status) tBox.BackColor = Color.Lime;
                                    else tBox.BackColor = Color.Red;
                                }
                                else tBox.BackColor = Color.Red;
                            }
                            else tBox.BackColor = Color.Red;
                        }
                    }
                }
            }
            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Green;
                label_tagFound.Text = "Tag written";
            });

            _aTimer.Enabled = true;
            mutexObj.Release();
        }

        private void checkBox_tagHex2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_tagHex.Checked)
            {
                textBox_tagEdit1.Text = Accessory.ConvertStringToHex(textBox_tagEdit1.Text);
                textBox_tagEdit2.Text = Accessory.ConvertStringToHex(textBox_tagEdit2.Text);
                textBox_tagEdit3.Text = Accessory.ConvertStringToHex(textBox_tagEdit3.Text);
                textBox_tagEdit4.Text = Accessory.ConvertStringToHex(textBox_tagEdit4.Text);
                textBox_tagEdit5.Text = Accessory.ConvertStringToHex(textBox_tagEdit5.Text);
                textBox_tagEdit6.Text = Accessory.ConvertStringToHex(textBox_tagEdit6.Text);
                textBox_tagEdit7.Text = Accessory.ConvertStringToHex(textBox_tagEdit7.Text);
            }
            else
            {
                textBox_tagEdit1.Text = Accessory.ConvertHexToString(textBox_tagEdit1.Text);
                textBox_tagEdit2.Text = Accessory.ConvertHexToString(textBox_tagEdit2.Text);
                textBox_tagEdit3.Text = Accessory.ConvertHexToString(textBox_tagEdit3.Text);
                textBox_tagEdit4.Text = Accessory.ConvertHexToString(textBox_tagEdit4.Text);
                textBox_tagEdit5.Text = Accessory.ConvertHexToString(textBox_tagEdit5.Text);
                textBox_tagEdit6.Text = Accessory.ConvertHexToString(textBox_tagEdit6.Text);
                textBox_tagEdit7.Text = Accessory.ConvertHexToString(textBox_tagEdit7.Text);
            }
            textBox_tagEdit1_Leave(this, EventArgs.Empty);
            textBox_tagEdit2_Leave(this, EventArgs.Empty);
            textBox_tagEdit3_Leave(this, EventArgs.Empty);
            textBox_tagEdit4_Leave(this, EventArgs.Empty);
            textBox_tagEdit5_Leave(this, EventArgs.Empty);
            textBox_tagEdit6_Leave(this, EventArgs.Empty);
            textBox_tagEdit7_Leave(this, EventArgs.Empty);
        }

        private void textBox_tagEdit1_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit1.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit1.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit1.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit1.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit2_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit2.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit2.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit2.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit2.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit3_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit3.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit3.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit3.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit3.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit4_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit4.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit4.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit4.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit4.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit5_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit5.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit5.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit5.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit5.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit6_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit6.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit6.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit6.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit6.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_tagEdit7_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_tagHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_tagEdit7.Text);
            else tmpStr = Accessory.CheckHexString(textBox_tagEdit7.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_pageSize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
            else outp = tmp;

            if (checkBox_tagHex.Checked) textBox_tagEdit7.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_tagEdit7.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
        }

        private void textBox_keyA_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_keyHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_keyA.Text);
            else tmpStr = Accessory.CheckHexString(textBox_keyA.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_keySize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _keySize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _keySize) Array.Copy(tmp, outp, _keySize);
            else outp = tmp;

            if (checkBox_keyHex.Checked) textBox_keyA.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_keyA.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
            _defaultKeyA = outp;
        }

        private void textBox_keyB_Leave(object sender, EventArgs e)
        {
            string tmpStr = "";
            if (!checkBox_keyHex.Checked) tmpStr = Accessory.ConvertStringToHex(textBox_keyB.Text);
            else tmpStr = Accessory.CheckHexString(textBox_keyB.Text);

            byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
            byte[] outp = new byte[_keySize];
            for (int i = 0; i < outp.Length; i++) outp[i] = 0xff;
            if (tmp.Length < _keySize) Array.Copy(tmp, outp, tmp.Length);
            else if (tmp.Length > _keySize) Array.Copy(tmp, outp, _keySize);
            else outp = tmp;

            if (checkBox_keyHex.Checked) textBox_keyB.Text = Accessory.ConvertByteArrayToHex(outp);
            else textBox_keyB.Text = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp));
            _defaultKeyB = outp;
        }

        private void checkBox_authA_CheckedChanged(object sender, EventArgs e)
        {
            textBox_keyA.Enabled = checkBox_authA.Checked;
            if (checkBox_authA.Checked)
            {
                _defaultKeyA = Accessory.ConvertHexToByteArray(textBox_keyA.Text);
            }
            else _defaultKeyA = null;
        }

        private void checkBox_authB_CheckedChanged(object sender, EventArgs e)
        {
            textBox_keyB.Enabled = checkBox_authB.Checked;
            if (checkBox_authB.Checked)
            {
                _defaultKeyB = Accessory.ConvertHexToByteArray(textBox_keyB.Text);
            }
            else _defaultKeyB = null;
        }

        private void button_hardReset_Click(object sender, EventArgs e)
        {
            reader.PCD_HwReset();
        }

        private void checkBox_page1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit1.Enabled = checkBox_page1.Checked;
        }

        private void checkBox_page2_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit2.Enabled = checkBox_page2.Checked;
        }

        private void checkBox_page3_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit3.Enabled = checkBox_page3.Checked;
        }

        private void checkBox_page4_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit4.Enabled = checkBox_page4.Checked;
        }

        private void checkBox_page5_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit5.Enabled = checkBox_page5.Checked;
        }

        private void checkBox_page6_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit6.Enabled = checkBox_page6.Checked;
        }

        private void checkBox_page7_CheckedChanged(object sender, EventArgs e)
        {
            textBox_tagEdit7.Enabled = checkBox_page7.Checked;
        }

    }
}
