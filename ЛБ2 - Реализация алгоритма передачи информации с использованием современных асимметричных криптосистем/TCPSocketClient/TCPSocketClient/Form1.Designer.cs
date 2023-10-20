namespace TCPSocketClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.lbHistory = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bConnect = new System.Windows.Forms.Button();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtbOpenKey = new System.Windows.Forms.RichTextBox();
            this.keyLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(671, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 20);
            this.button2.TabIndex = 6;
            this.button2.Text = "Отправить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Сообщение для передачи";
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(154, 344);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(511, 20);
            this.tbMessage.TabIndex = 8;
            this.tbMessage.Text = "Привет от Александра Астафьева!!!";
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            // 
            // lbHistory
            // 
            this.lbHistory.FormattingEnabled = true;
            this.lbHistory.Location = new System.Drawing.Point(12, 87);
            this.lbHistory.Name = "lbHistory";
            this.lbHistory.Size = new System.Drawing.Size(776, 251);
            this.lbHistory.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bConnect);
            this.groupBox1.Controls.Add(this.tbAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 58);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры подключения";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(389, 19);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(155, 20);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "12345";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт подключения";
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(550, 10);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(220, 37);
            this.bConnect.TabIndex = 0;
            this.bConnect.Text = "Подключиться";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(120, 19);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(155, 20);
            this.tbAddress.TabIndex = 1;
            this.tbAddress.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Адрес подключения";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtbOpenKey);
            this.groupBox2.Controls.Add(this.keyLabel);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(12, 370);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 180);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Подсиситема асимметричного шифрования";
            // 
            // rtbOpenKey
            // 
            this.rtbOpenKey.Location = new System.Drawing.Point(9, 57);
            this.rtbOpenKey.Name = "rtbOpenKey";
            this.rtbOpenKey.Size = new System.Drawing.Size(761, 117);
            this.rtbOpenKey.TabIndex = 2;
            this.rtbOpenKey.Text = "";
            // 
            // keyLabel
            // 
            this.keyLabel.AutoSize = true;
            this.keyLabel.ForeColor = System.Drawing.Color.Red;
            this.keyLabel.Location = new System.Drawing.Point(347, 29);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(97, 13);
            this.keyLabel.TabIndex = 1;
            this.keyLabel.Text = "Ключ не доступен";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(332, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Запросить открытый ключ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 562);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.lbHistory);
            this.Name = "Form1";
            this.Text = "Клиентская часть приложения на основе TCP-сокетов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.ListBox lbHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.RichTextBox rtbOpenKey;
    }
}

