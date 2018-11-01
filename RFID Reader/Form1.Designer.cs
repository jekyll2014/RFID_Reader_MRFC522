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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.checkBox_tagHex = new System.Windows.Forms.CheckBox();
            this.button_read = new System.Windows.Forms.Button();
            this.button_write = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.dataGridView_data = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_data)).BeginInit();
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
            // dataGridView_data
            // 
            this.dataGridView_data.AllowUserToAddRows = false;
            this.dataGridView_data.AllowUserToDeleteRows = false;
            this.dataGridView_data.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView_data.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_data.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_data.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView_data.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView_data.Location = new System.Drawing.Point(172, 188);
            this.dataGridView_data.MultiSelect = false;
            this.dataGridView_data.Name = "dataGridView_data";
            this.dataGridView_data.ReadOnly = true;
            this.dataGridView_data.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView_data.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_data.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView_data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_data.ShowEditingIcon = false;
            this.dataGridView_data.Size = new System.Drawing.Size(932, 281);
            this.dataGridView_data.TabIndex = 51;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1116, 481);
            this.Controls.Add(this.dataGridView_data);
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
            this.Controls.Add(this.comboBox_portName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_speed);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.button_refersh);
            this.Controls.Add(this.button_hardReset);
            this.Controls.Add(this.checkBox_tagHex);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "RFID Reader";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_data)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBox_tagHex;
        private System.Windows.Forms.DataGridView dataGridView_data;
    }
}

