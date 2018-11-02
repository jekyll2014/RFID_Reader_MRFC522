using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RFID_Reader
{
    public partial class Form1 : Form
    {
        private List<byte[]> cardData = new List<byte[]>();
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

        private byte[][] _defaultKeys = new byte[][]
{
            new byte[6]{0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, // FF FF FF FF FF FF
            new byte[6]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, // 00 00 00 00 00 00
            new byte[6]{0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5}, // A0 A1 A2 A3 A4 A5
            new byte[6]{0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5}, // B0 B1 B2 B3 B4 B5
            new byte[6]{0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0}, // a0 b0 c0 d0 e0 f0
            new byte[6]{0xa1, 0xb1, 0xc1, 0xd1, 0xe1, 0xf1}, // a1 b1 c1 d1 e1 f1
            new byte[6]{0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff}, // AA BB CC DD EE FF
            new byte[6]{0x4d, 0x3a, 0x99, 0xc3, 0x51, 0xdd}, // 4D 3A 99 C3 51 DD
            new byte[6]{0x1a, 0x98, 0x2c, 0x7e, 0x45, 0x9a}, // 1A 98 2C 7E 45 9A
            new byte[6]{0xd3, 0xf7, 0xd3, 0xf7, 0xd3, 0xf7}, // D3 F7 D3 F7 D3 F7
            new byte[6]{0x71, 0x4c, 0x5c, 0x88, 0x6e, 0x97}, // 71 4c 5c 88 6e 97
            new byte[6]{0x58, 0x7e, 0xe5, 0xf9, 0x35, 0x0f}, // 58 7e e5 f9 35 0f
            new byte[6]{0xa0, 0x47, 0x8c, 0xc3, 0x90, 0x91}, // a0 47 8c c3 90 91
            new byte[6]{0x53, 0x3c, 0xb6, 0xc7, 0x23, 0xf6}, // 53 3c b6 c7 23 f6
            new byte[6]{0x8f, 0xd0, 0xa4, 0xf2, 0x56, 0xe9}, // 8f d0 a4 f2 56 e9
};

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
                    if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_MINI || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_1K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_4K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_PLUS)
                    {
                        // This is the default key for authentication
                        byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                        if (keyA != null && keyA.Length != 6)
                        {
                            keyA = defaultKey;
                        }
                        if (keyB != null && keyB.Length != 6)
                        {
                            keyB = defaultKey;
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
                    else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_UL || t == MFRC522.PICC_Type.PICC_TYPE_ISO_14443_4)
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
            _aTimer.Enabled = false;
            mutexObj.WaitOne(_tmr_delay);

            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Yellow;
                label_tagFound.Text = "Reading tag";
                this.Refresh();
            });
            checkBox_dataHex.Checked = true;

            Thread t = new Thread(new ThreadStart(getDataFromRFID));
            t.Start();

        }

        private void getDataFromRFID()
        {
            this.Invoke((MethodInvoker)delegate
            {
                button_start.Enabled = false;
                button_hardReset.Enabled = false;
                button_read.Enabled = false;
                button_write.Enabled = false;
                button_clear.Enabled = false;
            });

            byte[] uid = RFID_hunt();
            MFRC522.PICC_Type t = reader.PICC_GetType(reader.uid.sak);
            byte pageNum = 0;
            if (uid != null)
            {
                if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_UL || t == MFRC522.PICC_Type.PICC_TYPE_ISO_14443_4)
                {
                    pageNum = 48;
                    List<byte[]> data = new List<byte[]>();
                    data = RFID_read(uid, null, null, 0, (byte)(pageNum - 1));
                    cardData.AddRange(data);
                    this.Invoke((MethodInvoker)delegate
                    {
                        dataGridView_data.Rows.Clear();
                        dataGridView_data.Columns.Clear();
                        dataGridView_data.Columns.Add("num", "#");
                        dataGridView_data.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                        DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
                        checkCol.Name = "check";
                        checkCol.HeaderText = "Writable";
                        dataGridView_data.Columns.Add(checkCol);
                        dataGridView_data.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                        dataGridView_data.Columns.Add("data", "Card data");
                        dataGridView_data.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        int i = 0;
                        foreach (var d in data)
                        {
                            dataGridView_data.Rows.Add();
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[0].Value = i.ToString("D3");
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[0].ReadOnly = true;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[1].Value = false;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[1].Style.BackColor = Color.White;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[2].Value = Accessory.ConvertByteArrayToHex(d);
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[2].ReadOnly = true;
                            i++;
                            this.Refresh();
                        }
                    });
                }
                else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_MINI || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_1K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_4K || t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_PLUS)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        dataGridView_data.Rows.Clear();
                        dataGridView_data.Columns.Clear();
                        dataGridView_data.Columns.Add("num", "#");
                        dataGridView_data.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                        DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
                        checkCol.Name = "check";
                        checkCol.HeaderText = "Writable";
                        dataGridView_data.Columns.Add(checkCol);
                        dataGridView_data.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;


                        dataGridView_data.Columns.Add("data", "Card data");
                        dataGridView_data.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

                        dataGridView_data.Columns.Add("keyA", "KEY A");
                        dataGridView_data.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dataGridView_data.Columns.Add("keyB", "KEY B");
                        dataGridView_data.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    });
                    if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_MINI) pageNum = 5 * 4; //Mini
                    else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_1K) pageNum = 16 * 4; //1K
                    else if (t == MFRC522.PICC_Type.PICC_TYPE_MIFARE_4K) pageNum = 40 * 4; //4K
                    else pageNum = 40 * 4; //PLUS

                    byte[][] useKeys = new byte[][] { new byte[] { } };
                    if (checkBox_authA.Checked && !checkBox_authB.Checked) useKeys = new byte[][] { _defaultKeyA };
                    else if (!checkBox_authA.Checked && checkBox_authB.Checked) useKeys = new byte[][] { _defaultKeyB };
                    else if (checkBox_authA.Checked && checkBox_authB.Checked) useKeys = useKeys = new byte[][] { _defaultKeyA, _defaultKeyB };
                    else if (!checkBox_authA.Checked && !checkBox_authB.Checked)
                    {
                        useKeys = _defaultKeys;
                        this.Invoke((MethodInvoker)delegate
                        {
                            label_tagFound.BackColor = Color.Yellow;
                            label_tagFound.Text = "Reading tag\r\nTrying default keys...";
                            this.Refresh();
                        });
                    }
                    byte p = 0;
                    for (byte b = 0; b < pageNum; b++)
                    {
                        int k = 0;
                        bool stage = false;
                        List<byte[]> data = new List<byte[]>();
                        do
                        {
                            if (stage == false) data = RFID_read(uid, useKeys[k], null, b, b);
                            else data = RFID_read(uid, null, useKeys[k], b, b);
                            k++;
                            if (data[0] == null && k >= useKeys.Length && stage == false)
                            {
                                stage = true;
                                k = 0;
                            }
                        } while (data[0] == null && k < useKeys.Length);
                        cardData.AddRange(data);

                        this.Invoke((MethodInvoker)delegate
                        {
                            dataGridView_data.Rows.Add();
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[0].Value = b.ToString("D3");
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[0].ReadOnly = true;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[1].Value = false;
                            p++;
                            if (p >= 4)
                            {
                                dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[1].Style.BackColor = Color.Red;
                                p = 0;
                            }

                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[2].Value = Accessory.ConvertByteArrayToHex(data[0]);
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[2].ReadOnly = true;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[3].ReadOnly = true;
                            dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[4].ReadOnly = true;
                            if (data[0] != null)
                            {
                                if (stage) dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[4].Value = Accessory.ConvertByteArrayToHex(useKeys[k - 1]);
                                else dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[3].Value = Accessory.ConvertByteArrayToHex(useKeys[k - 1]);
                                raiseKey(ref useKeys, k - 1);
                            }
                        });
                    }
                }
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView_data.Rows.Clear();
                    dataGridView_data.Columns.Clear();
                    dataGridView_data.Columns.Add("num", "");
                    dataGridView_data.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dataGridView_data.Rows.Add();
                    dataGridView_data.Rows[dataGridView_data.Rows.Count - 1].Cells[0].Value = "Can't find tag";
                });
            }

            this.Invoke((MethodInvoker)delegate
            {
                button_start.Enabled = true;
                button_hardReset.Enabled = true;
                button_read.Enabled = true;
                button_write.Enabled = true;
                button_clear.Enabled = true;
            });
            mutexObj.Release();
            _aTimer.Enabled = true;
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
            checkBox_dataHex.Checked = true;

            for (byte i = 0; i < dataGridView_data.Rows.Count; i++)
            {
                if (dataGridView_data.Rows[i].Cells[1].Value != null && (bool)dataGridView_data.Rows[i].Cells[1].Value == true)
                {
                    byte[] uid = null;
                    uid = RFID_hunt();
                    if (uid != null)
                    {
                        bool status = RFID_write(uid, _defaultKeyA, _defaultKeyB, i, cardData[i]);
                        if (status) dataGridView_data.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        else dataGridView_data.Rows[i].Cells[2].Style.BackColor = Color.Red;
                    }
                    else dataGridView_data.Rows[i].Cells[2].Style.BackColor = Color.Red;
                }
                /*Control[] controls = Controls.Find("checkBox_page" + i.ToString(), true);
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
                }*/
            }
            this.Invoke((MethodInvoker)delegate
            {
                label_tagFound.BackColor = Color.Green;
                label_tagFound.Text = "Tag written";
            });

            _aTimer.Enabled = true;
            mutexObj.Release();
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
                textBox_keyA_Leave(this, EventArgs.Empty);
                _defaultKeyA = Accessory.ConvertHexToByteArray(textBox_keyA.Text);
            }
            else _defaultKeyA = null;
        }

        private void checkBox_authB_CheckedChanged(object sender, EventArgs e)
        {
            textBox_keyB.Enabled = checkBox_authB.Checked;
            if (checkBox_authB.Checked)
            {
                textBox_keyB_Leave(this, EventArgs.Empty);
                _defaultKeyB = Accessory.ConvertHexToByteArray(textBox_keyB.Text);
            }
            else _defaultKeyB = null;
        }

        private void button_hardReset_Click(object sender, EventArgs e)
        {
            reader.PCD_HwReset();
        }

        private void raiseKey(ref byte[][] array, int n)
        {
            if (n <= 0) return;
            byte[] tmpKey = array[n];
            for (int i = n; i > 0; i--)
            {
                array[i] = array[i - 1];
            }
            array[0] = tmpKey;
        }

        private void dataGridView_data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_data.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView_data_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
            {
                if ((bool)dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == true)
                {
                    dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = false;
                }
                else
                {
                    dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = true;
                }
            }
            else if (e.ColumnIndex == 2)
            {
                string tmpStr = "";
                if (!checkBox_dataHex.Checked) tmpStr = Accessory.ConvertStringToHex(dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                else tmpStr = Accessory.CheckHexString(dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                byte[] tmp = Accessory.ConvertHexToByteArray(tmpStr);
                byte[] outp = new byte[_pageSize];
                for (int i = 0; i < outp.Length; i++) outp[i] = 0x00;
                if (tmp.Length < _pageSize) Array.Copy(tmp, outp, tmp.Length);
                else if (tmp.Length > _pageSize) Array.Copy(tmp, outp, _pageSize);
                else outp = tmp;
                cardData[e.RowIndex] = outp;

                if (checkBox_dataHex.Checked) dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Accessory.ConvertByteArrayToHex(outp);
                else dataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Accessory.ConvertHexToString(Accessory.ConvertByteArrayToHex(outp)).Replace((char)0, ' ');
            }
        }

        private void checkBox_keyHex_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_keyHex.Checked)
            {
                textBox_keyA.Text = Accessory.ConvertByteArrayToHex(_defaultKeyA);
                textBox_keyB.Text = Accessory.ConvertByteArrayToHex(_defaultKeyB);
            }
            else
            {
                textBox_keyA.Text = Accessory.ConvertByteArrayToString(_defaultKeyA);
                textBox_keyB.Text = Accessory.ConvertByteArrayToString(_defaultKeyB);
            }
        }

        private void checkBox_dataHex_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_dataHex.Checked)
            {
                for (int i = 0; i < dataGridView_data.Rows.Count; i++)
                    dataGridView_data.Rows[i].Cells[2].Value = Accessory.ConvertByteArrayToHex(cardData[i]);
            }
            else
            {
                for (int i = 0; i < dataGridView_data.Rows.Count; i++)
                    dataGridView_data.Rows[i].Cells[2].Value = Accessory.ConvertByteArrayToString(cardData[i]).Replace((char)0, ' ');
            }
        }

    }
}
