namespace RFID_Reader
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_hardReset = new System.Windows.Forms.Button();
            this.button_refersh = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBox_speed = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_portName = new System.Windows.Forms.ComboBox();
            this.button_start = new System.Windows.Forms.Button();
            this.checkBox_authB = new System.Windows.Forms.CheckBox();
            this.checkBox_authA = new System.Windows.Forms.CheckBox();
            this.checkBox_keyHex = new System.Windows.Forms.CheckBox();
            this.textBox_keyB = new System.Windows.Forms.TextBox();
            this.textBox_keyA = new System.Windows.Forms.TextBox();
            this.textBox_tagUUID = new System.Windows.Forms.TextBox();
            this.label_tagUID = new System.Windows.Forms.Label();
            this.textBox_tagType = new System.Windows.Forms.TextBox();
            this.label_tagType = new System.Windows.Forms.Label();
            this.label_tagFound = new System.Windows.Forms.Label();
            this.checkBox_page8 = new System.Windows.Forms.CheckBox();
            this.checkBox_page7 = new System.Windows.Forms.CheckBox();
            this.checkBox_page6 = new System.Windows.Forms.CheckBox();
            this.checkBox_page5 = new System.Windows.Forms.CheckBox();
            this.checkBox_page4 = new System.Windows.Forms.CheckBox();
            this.checkBox_page3 = new System.Windows.Forms.CheckBox();
            this.checkBox_page2 = new System.Windows.Forms.CheckBox();
            this.checkBox_page1 = new System.Windows.Forms.CheckBox();
            this.checkBox_tagHex = new System.Windows.Forms.CheckBox();
            this.textBox_tagEdit8 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit7 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit6 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit5 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit4 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit3 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit2 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit1 = new System.Windows.Forms.TextBox();
            this.textBox_tagEdit0 = new System.Windows.Forms.TextBox();
            this.button_read = new System.Windows.Forms.Button();
            this.button_write = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.checkBox_page0 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_hardReset
            // 
            this.button_hardReset.Location = new System.Drawing.Point(12, 339);
            this.button_hardReset.Name = "button_hardReset";
            this.button_hardReset.Size = new System.Drawing.Size(151, 28);
            this.button_hardReset.TabIndex = 21;
            this.button_hardReset.Text = "Hard reset (DTR)";
            this.button_hardReset.UseVisualStyleBackColor = true;
            this.button_hardReset.Click += new System.EventHandler(this.button_hardReset_Click);
            // 
            // button_refersh
            // 
            this.button_refersh.Location = new System.Drawing.Point(12, 162);
            this.button_refersh.Name = "button_refersh";
            this.button_refersh.Size = new System.Drawing.Size(151, 28);
            this.button_refersh.TabIndex = 21;
            this.button_refersh.Text = "Refresh ports";
            this.button_refersh.UseVisualStyleBackColor = true;
            this.button_refersh.Click += new System.EventHandler(this.button_refersh_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(16, 239);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 20);
            this.label17.TabIndex = 20;
            this.label17.Text = "Speed";
            // 
            // comboBox_speed
            // 
            this.comboBox_speed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_speed.FormattingEnabled = true;
            this.comboBox_speed.Items.AddRange(new object[] {
            "1228800",
            "921600",
            "460800",
            "230400",
            "115200",
            "57600",
            "38400",
            "19200",
            "9600",
            "7200"});
            this.comboBox_speed.Location = new System.Drawing.Point(74, 236);
            this.comboBox_speed.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_speed.Name = "comboBox_speed";
            this.comboBox_speed.Size = new System.Drawing.Size(89, 28);
            this.comboBox_speed.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 199);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Port #";
            // 
            // comboBox_portName
            // 
            this.comboBox_portName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_portName.FormattingEnabled = true;
            this.comboBox_portName.Location = new System.Drawing.Point(74, 196);
            this.comboBox_portName.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_portName.Name = "comboBox_portName";
            this.comboBox_portName.Size = new System.Drawing.Size(89, 28);
            this.comboBox_portName.TabIndex = 16;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(12, 273);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(151, 60);
            this.button_start.TabIndex = 14;
            this.button_start.Text = "Start reading";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // checkBox_authB
            // 
            this.checkBox_authB.AutoSize = true;
            this.checkBox_authB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_authB.Location = new System.Drawing.Point(175, 121);
            this.checkBox_authB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_authB.Name = "checkBox_authB";
            this.checkBox_authB.Size = new System.Drawing.Size(65, 24);
            this.checkBox_authB.TabIndex = 3;
            this.checkBox_authB.Text = "KeyB";
            this.checkBox_authB.UseVisualStyleBackColor = true;
            this.checkBox_authB.CheckedChanged += new System.EventHandler(this.checkBox_authB_CheckedChanged);
            // 
            // checkBox_authA
            // 
            this.checkBox_authA.AutoSize = true;
            this.checkBox_authA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_authA.Checked = true;
            this.checkBox_authA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_authA.Location = new System.Drawing.Point(175, 85);
            this.checkBox_authA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_authA.Name = "checkBox_authA";
            this.checkBox_authA.Size = new System.Drawing.Size(65, 24);
            this.checkBox_authA.TabIndex = 3;
            this.checkBox_authA.Text = "KeyA";
            this.checkBox_authA.UseVisualStyleBackColor = true;
            this.checkBox_authA.CheckedChanged += new System.EventHandler(this.checkBox_authA_CheckedChanged);
            // 
            // checkBox_keyHex
            // 
            this.checkBox_keyHex.AutoSize = true;
            this.checkBox_keyHex.Checked = true;
            this.checkBox_keyHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_keyHex.Location = new System.Drawing.Point(279, 155);
            this.checkBox_keyHex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_keyHex.Name = "checkBox_keyHex";
            this.checkBox_keyHex.Size = new System.Drawing.Size(81, 24);
            this.checkBox_keyHex.TabIndex = 3;
            this.checkBox_keyHex.Text = "hex key";
            this.checkBox_keyHex.UseVisualStyleBackColor = true;
            // 
            // textBox_keyB
            // 
            this.textBox_keyB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_keyB.Enabled = false;
            this.textBox_keyB.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_keyB.Location = new System.Drawing.Point(279, 119);
            this.textBox_keyB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_keyB.Multiline = true;
            this.textBox_keyB.Name = "textBox_keyB";
            this.textBox_keyB.Size = new System.Drawing.Size(824, 26);
            this.textBox_keyB.TabIndex = 2;
            this.textBox_keyB.Text = "FF FF FF FF FF FF ";
            this.textBox_keyB.Leave += new System.EventHandler(this.textBox_keyB_Leave);
            // 
            // textBox_keyA
            // 
            this.textBox_keyA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_keyA.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_keyA.Location = new System.Drawing.Point(279, 83);
            this.textBox_keyA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_keyA.Multiline = true;
            this.textBox_keyA.Name = "textBox_keyA";
            this.textBox_keyA.Size = new System.Drawing.Size(824, 26);
            this.textBox_keyA.TabIndex = 2;
            this.textBox_keyA.Text = "FF FF FF FF FF FF ";
            this.textBox_keyA.Leave += new System.EventHandler(this.textBox_keyA_Leave);
            // 
            // textBox_tagUUID
            // 
            this.textBox_tagUUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagUUID.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagUUID.Location = new System.Drawing.Point(279, 47);
            this.textBox_tagUUID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagUUID.Name = "textBox_tagUUID";
            this.textBox_tagUUID.ReadOnly = true;
            this.textBox_tagUUID.Size = new System.Drawing.Size(824, 26);
            this.textBox_tagUUID.TabIndex = 2;
            // 
            // label_tagUID
            // 
            this.label_tagUID.AutoSize = true;
            this.label_tagUID.Location = new System.Drawing.Point(171, 50);
            this.label_tagUID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tagUID.Name = "label_tagUID";
            this.label_tagUID.Size = new System.Drawing.Size(85, 20);
            this.label_tagUID.TabIndex = 1;
            this.label_tagUID.Text = "Tag UUID:";
            // 
            // textBox_tagType
            // 
            this.textBox_tagType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagType.Location = new System.Drawing.Point(279, 14);
            this.textBox_tagType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagType.Name = "textBox_tagType";
            this.textBox_tagType.ReadOnly = true;
            this.textBox_tagType.Size = new System.Drawing.Size(824, 26);
            this.textBox_tagType.TabIndex = 2;
            // 
            // label_tagType
            // 
            this.label_tagType.AutoSize = true;
            this.label_tagType.Location = new System.Drawing.Point(171, 17);
            this.label_tagType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tagType.Name = "label_tagType";
            this.label_tagType.Size = new System.Drawing.Size(74, 20);
            this.label_tagType.TabIndex = 1;
            this.label_tagType.Text = "Tag type:";
            // 
            // label_tagFound
            // 
            this.label_tagFound.BackColor = System.Drawing.Color.Gray;
            this.label_tagFound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_tagFound.Location = new System.Drawing.Point(13, 9);
            this.label_tagFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tagFound.Name = "label_tagFound";
            this.label_tagFound.Size = new System.Drawing.Size(150, 150);
            this.label_tagFound.TabIndex = 0;
            this.label_tagFound.Text = "Tag not found";
            this.label_tagFound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox_page8
            // 
            this.checkBox_page8.AutoSize = true;
            this.checkBox_page8.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page8.Location = new System.Drawing.Point(175, 477);
            this.checkBox_page8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page8.Name = "checkBox_page8";
            this.checkBox_page8.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page8.TabIndex = 38;
            this.checkBox_page8.Text = "Page08";
            this.checkBox_page8.UseVisualStyleBackColor = true;
            this.checkBox_page8.CheckedChanged += new System.EventHandler(this.checkBox_page8_CheckedChanged);
            // 
            // checkBox_page7
            // 
            this.checkBox_page7.AutoSize = true;
            this.checkBox_page7.BackColor = System.Drawing.Color.Red;
            this.checkBox_page7.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page7.Location = new System.Drawing.Point(175, 441);
            this.checkBox_page7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page7.Name = "checkBox_page7";
            this.checkBox_page7.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page7.TabIndex = 37;
            this.checkBox_page7.Text = "Page07";
            this.checkBox_page7.UseVisualStyleBackColor = false;
            this.checkBox_page7.CheckedChanged += new System.EventHandler(this.checkBox_page7_CheckedChanged);
            // 
            // checkBox_page6
            // 
            this.checkBox_page6.AutoSize = true;
            this.checkBox_page6.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page6.Location = new System.Drawing.Point(175, 405);
            this.checkBox_page6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page6.Name = "checkBox_page6";
            this.checkBox_page6.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page6.TabIndex = 36;
            this.checkBox_page6.Text = "Page06";
            this.checkBox_page6.UseVisualStyleBackColor = true;
            this.checkBox_page6.CheckedChanged += new System.EventHandler(this.checkBox_page6_CheckedChanged);
            // 
            // checkBox_page5
            // 
            this.checkBox_page5.AutoSize = true;
            this.checkBox_page5.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page5.Location = new System.Drawing.Point(175, 369);
            this.checkBox_page5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page5.Name = "checkBox_page5";
            this.checkBox_page5.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page5.TabIndex = 39;
            this.checkBox_page5.Text = "Page05";
            this.checkBox_page5.UseVisualStyleBackColor = true;
            this.checkBox_page5.CheckedChanged += new System.EventHandler(this.checkBox_page5_CheckedChanged);
            // 
            // checkBox_page4
            // 
            this.checkBox_page4.AutoSize = true;
            this.checkBox_page4.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page4.Location = new System.Drawing.Point(175, 333);
            this.checkBox_page4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page4.Name = "checkBox_page4";
            this.checkBox_page4.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page4.TabIndex = 35;
            this.checkBox_page4.Text = "Page04";
            this.checkBox_page4.UseVisualStyleBackColor = true;
            this.checkBox_page4.CheckedChanged += new System.EventHandler(this.checkBox_page4_CheckedChanged);
            // 
            // checkBox_page3
            // 
            this.checkBox_page3.AutoSize = true;
            this.checkBox_page3.BackColor = System.Drawing.Color.Red;
            this.checkBox_page3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page3.Location = new System.Drawing.Point(175, 297);
            this.checkBox_page3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page3.Name = "checkBox_page3";
            this.checkBox_page3.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page3.TabIndex = 34;
            this.checkBox_page3.Text = "Page03";
            this.checkBox_page3.UseVisualStyleBackColor = false;
            this.checkBox_page3.CheckedChanged += new System.EventHandler(this.checkBox_page3_CheckedChanged);
            // 
            // checkBox_page2
            // 
            this.checkBox_page2.AutoSize = true;
            this.checkBox_page2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page2.Location = new System.Drawing.Point(175, 261);
            this.checkBox_page2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page2.Name = "checkBox_page2";
            this.checkBox_page2.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page2.TabIndex = 33;
            this.checkBox_page2.Text = "Page02";
            this.checkBox_page2.UseVisualStyleBackColor = true;
            this.checkBox_page2.CheckedChanged += new System.EventHandler(this.checkBox_page2_CheckedChanged);
            // 
            // checkBox_page1
            // 
            this.checkBox_page1.AutoSize = true;
            this.checkBox_page1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page1.Location = new System.Drawing.Point(175, 225);
            this.checkBox_page1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page1.Name = "checkBox_page1";
            this.checkBox_page1.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page1.TabIndex = 32;
            this.checkBox_page1.Text = "Page01";
            this.checkBox_page1.UseVisualStyleBackColor = true;
            this.checkBox_page1.CheckedChanged += new System.EventHandler(this.checkBox_page1_CheckedChanged);
            // 
            // checkBox_tagHex
            // 
            this.checkBox_tagHex.AutoSize = true;
            this.checkBox_tagHex.Checked = true;
            this.checkBox_tagHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_tagHex.Location = new System.Drawing.Point(508, 155);
            this.checkBox_tagHex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_tagHex.Name = "checkBox_tagHex";
            this.checkBox_tagHex.Size = new System.Drawing.Size(89, 24);
            this.checkBox_tagHex.TabIndex = 31;
            this.checkBox_tagHex.Text = "hex data";
            this.checkBox_tagHex.UseVisualStyleBackColor = true;
            this.checkBox_tagHex.CheckedChanged += new System.EventHandler(this.checkBox_tagHex2_CheckedChanged);
            // 
            // textBox_tagEdit8
            // 
            this.textBox_tagEdit8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit8.Enabled = false;
            this.textBox_tagEdit8.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit8.Location = new System.Drawing.Point(279, 477);
            this.textBox_tagEdit8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit8.Name = "textBox_tagEdit8";
            this.textBox_tagEdit8.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit8.TabIndex = 29;
            this.textBox_tagEdit8.Leave += new System.EventHandler(this.textBox_tagEdit8_Leave);
            // 
            // textBox_tagEdit7
            // 
            this.textBox_tagEdit7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit7.Enabled = false;
            this.textBox_tagEdit7.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit7.Location = new System.Drawing.Point(279, 441);
            this.textBox_tagEdit7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit7.Name = "textBox_tagEdit7";
            this.textBox_tagEdit7.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit7.TabIndex = 28;
            this.textBox_tagEdit7.Leave += new System.EventHandler(this.textBox_tagEdit7_Leave);
            // 
            // textBox_tagEdit6
            // 
            this.textBox_tagEdit6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit6.Enabled = false;
            this.textBox_tagEdit6.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit6.Location = new System.Drawing.Point(279, 405);
            this.textBox_tagEdit6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit6.Name = "textBox_tagEdit6";
            this.textBox_tagEdit6.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit6.TabIndex = 27;
            this.textBox_tagEdit6.Leave += new System.EventHandler(this.textBox_tagEdit6_Leave);
            // 
            // textBox_tagEdit5
            // 
            this.textBox_tagEdit5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit5.Enabled = false;
            this.textBox_tagEdit5.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit5.Location = new System.Drawing.Point(279, 369);
            this.textBox_tagEdit5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit5.Name = "textBox_tagEdit5";
            this.textBox_tagEdit5.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit5.TabIndex = 26;
            this.textBox_tagEdit5.Leave += new System.EventHandler(this.textBox_tagEdit5_Leave);
            // 
            // textBox_tagEdit4
            // 
            this.textBox_tagEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit4.Enabled = false;
            this.textBox_tagEdit4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit4.Location = new System.Drawing.Point(279, 333);
            this.textBox_tagEdit4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit4.Name = "textBox_tagEdit4";
            this.textBox_tagEdit4.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit4.TabIndex = 25;
            this.textBox_tagEdit4.Leave += new System.EventHandler(this.textBox_tagEdit4_Leave);
            // 
            // textBox_tagEdit3
            // 
            this.textBox_tagEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit3.Enabled = false;
            this.textBox_tagEdit3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit3.Location = new System.Drawing.Point(279, 297);
            this.textBox_tagEdit3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit3.Name = "textBox_tagEdit3";
            this.textBox_tagEdit3.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit3.TabIndex = 24;
            this.textBox_tagEdit3.Leave += new System.EventHandler(this.textBox_tagEdit3_Leave);
            // 
            // textBox_tagEdit2
            // 
            this.textBox_tagEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit2.Enabled = false;
            this.textBox_tagEdit2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit2.Location = new System.Drawing.Point(279, 261);
            this.textBox_tagEdit2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit2.Name = "textBox_tagEdit2";
            this.textBox_tagEdit2.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit2.TabIndex = 23;
            this.textBox_tagEdit2.Leave += new System.EventHandler(this.textBox_tagEdit2_Leave);
            // 
            // textBox_tagEdit1
            // 
            this.textBox_tagEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit1.Enabled = false;
            this.textBox_tagEdit1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit1.Location = new System.Drawing.Point(279, 225);
            this.textBox_tagEdit1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit1.Name = "textBox_tagEdit1";
            this.textBox_tagEdit1.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit1.TabIndex = 30;
            this.textBox_tagEdit1.Leave += new System.EventHandler(this.textBox_tagEdit1_Leave);
            // 
            // textBox_tagEdit0
            // 
            this.textBox_tagEdit0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_tagEdit0.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_tagEdit0.Enabled = false;
            this.textBox_tagEdit0.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_tagEdit0.Location = new System.Drawing.Point(279, 189);
            this.textBox_tagEdit0.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_tagEdit0.Name = "textBox_tagEdit0";
            this.textBox_tagEdit0.ReadOnly = true;
            this.textBox_tagEdit0.Size = new System.Drawing.Size(830, 21);
            this.textBox_tagEdit0.TabIndex = 22;
            this.textBox_tagEdit0.Leave += new System.EventHandler(this.textBox_tagEdit0_Leave);
            // 
            // button_read
            // 
            this.button_read.Enabled = false;
            this.button_read.Location = new System.Drawing.Point(12, 373);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(151, 28);
            this.button_read.TabIndex = 48;
            this.button_read.Text = "Read tag";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_write
            // 
            this.button_write.Enabled = false;
            this.button_write.Location = new System.Drawing.Point(12, 407);
            this.button_write.Name = "button_write";
            this.button_write.Size = new System.Drawing.Size(151, 28);
            this.button_write.TabIndex = 49;
            this.button_write.Text = "Write tag";
            this.button_write.UseVisualStyleBackColor = true;
            this.button_write.Click += new System.EventHandler(this.button_write_Click);
            // 
            // button_clear
            // 
            this.button_clear.Enabled = false;
            this.button_clear.Location = new System.Drawing.Point(12, 441);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(151, 28);
            this.button_clear.TabIndex = 50;
            this.button_clear.Text = "Clear tag";
            this.button_clear.UseVisualStyleBackColor = true;
            // 
            // checkBox_page0
            // 
            this.checkBox_page0.AutoSize = true;
            this.checkBox_page0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_page0.Enabled = false;
            this.checkBox_page0.Location = new System.Drawing.Point(175, 191);
            this.checkBox_page0.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_page0.Name = "checkBox_page0";
            this.checkBox_page0.Size = new System.Drawing.Size(83, 24);
            this.checkBox_page0.TabIndex = 32;
            this.checkBox_page0.Text = "Page00";
            this.checkBox_page0.UseVisualStyleBackColor = true;
            this.checkBox_page0.CheckedChanged += new System.EventHandler(this.checkBox_page0_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1116, 594);
            this.Controls.Add(this.button_read);
            this.Controls.Add(this.button_write);
            this.Controls.Add(this.label_tagFound);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.label_tagType);
            this.Controls.Add(this.textBox_tagType);
            this.Controls.Add(this.label_tagUID);
            this.Controls.Add(this.textBox_tagUUID);
            this.Controls.Add(this.textBox_keyA);
            this.Controls.Add(this.textBox_keyB);
            this.Controls.Add(this.checkBox_keyHex);
            this.Controls.Add(this.checkBox_authA);
            this.Controls.Add(this.checkBox_authB);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.checkBox_page8);
            this.Controls.Add(this.comboBox_portName);
            this.Controls.Add(this.checkBox_page7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_page6);
            this.Controls.Add(this.comboBox_speed);
            this.Controls.Add(this.checkBox_page5);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.checkBox_page4);
            this.Controls.Add(this.button_refersh);
            this.Controls.Add(this.checkBox_page3);
            this.Controls.Add(this.button_hardReset);
            this.Controls.Add(this.checkBox_page2);
            this.Controls.Add(this.textBox_tagEdit0);
            this.Controls.Add(this.checkBox_page0);
            this.Controls.Add(this.checkBox_page1);
            this.Controls.Add(this.textBox_tagEdit1);
            this.Controls.Add(this.checkBox_tagHex);
            this.Controls.Add(this.textBox_tagEdit2);
            this.Controls.Add(this.textBox_tagEdit8);
            this.Controls.Add(this.textBox_tagEdit3);
            this.Controls.Add(this.textBox_tagEdit7);
            this.Controls.Add(this.textBox_tagEdit4);
            this.Controls.Add(this.textBox_tagEdit6);
            this.Controls.Add(this.textBox_tagEdit5);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "RFID Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox_keyHex;
        private System.Windows.Forms.TextBox textBox_keyA;
        private System.Windows.Forms.TextBox textBox_tagUUID;
        private System.Windows.Forms.Label label_tagUID;
        private System.Windows.Forms.TextBox textBox_tagType;
        private System.Windows.Forms.Label label_tagType;
        private System.Windows.Forms.Label label_tagFound;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBox_speed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_portName;
        private System.Windows.Forms.TextBox textBox_keyB;
        private System.Windows.Forms.CheckBox checkBox_authB;
        private System.Windows.Forms.CheckBox checkBox_authA;
        private System.Windows.Forms.Button button_refersh;
        private System.Windows.Forms.Button button_hardReset;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.Button button_write;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.CheckBox checkBox_page8;
        private System.Windows.Forms.CheckBox checkBox_page7;
        private System.Windows.Forms.CheckBox checkBox_page6;
        private System.Windows.Forms.CheckBox checkBox_page5;
        private System.Windows.Forms.CheckBox checkBox_page4;
        private System.Windows.Forms.CheckBox checkBox_page3;
        private System.Windows.Forms.CheckBox checkBox_page2;
        private System.Windows.Forms.CheckBox checkBox_page1;
        private System.Windows.Forms.CheckBox checkBox_tagHex;
        private System.Windows.Forms.TextBox textBox_tagEdit8;
        private System.Windows.Forms.TextBox textBox_tagEdit7;
        private System.Windows.Forms.TextBox textBox_tagEdit6;
        private System.Windows.Forms.TextBox textBox_tagEdit5;
        private System.Windows.Forms.TextBox textBox_tagEdit4;
        private System.Windows.Forms.TextBox textBox_tagEdit3;
        private System.Windows.Forms.TextBox textBox_tagEdit2;
        private System.Windows.Forms.TextBox textBox_tagEdit1;
        private System.Windows.Forms.TextBox textBox_tagEdit0;
        private System.Windows.Forms.CheckBox checkBox_page0;
    }
}

