namespace TCPSocketServer
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
            this.bConnect = new System.Windows.Forms.Button();
            this.lbHistory = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(275, 10);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(495, 37);
            this.bConnect.TabIndex = 0;
            this.bConnect.Text = "Запустить сервер";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbHistory
            // 
            this.lbHistory.FormattingEnabled = true;
            this.lbHistory.Location = new System.Drawing.Point(12, 76);
            this.lbHistory.Name = "lbHistory";
            this.lbHistory.Size = new System.Drawing.Size(776, 251);
            this.lbHistory.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bConnect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 58);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры сервера";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(114, 19);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(155, 20);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "12345";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт подключения";
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(154, 333);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(511, 20);
            this.tbMessage.TabIndex = 4;
            this.tbMessage.Text = "Привет от Александра Астафьева!!!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 336);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Сообщение для передачи";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(671, 333);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 20);
            this.button2.TabIndex = 4;
            this.button2.Text = "Отправить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 367);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbHistory);
            this.Name = "Form1";
            this.Text = "Серверная часть приложения на основе TCP-сокетов";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.ListBox lbHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
    }
}

