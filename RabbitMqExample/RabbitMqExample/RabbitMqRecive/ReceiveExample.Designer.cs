namespace RabbitMqRecive
{
    partial class ReceiveExample
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.receiveTextBox = new System.Windows.Forms.TextBox();
            this.ButtonStarted = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // receiveTextBox
            // 
            this.receiveTextBox.Location = new System.Drawing.Point(12, 59);
            this.receiveTextBox.Multiline = true;
            this.receiveTextBox.Name = "receiveTextBox";
            this.receiveTextBox.Size = new System.Drawing.Size(776, 379);
            this.receiveTextBox.TabIndex = 0;
            // 
            // ButtonStarted
            // 
            this.ButtonStarted.Location = new System.Drawing.Point(12, 12);
            this.ButtonStarted.Name = "ButtonStarted";
            this.ButtonStarted.Size = new System.Drawing.Size(169, 34);
            this.ButtonStarted.TabIndex = 1;
            this.ButtonStarted.Text = "啟動接收訊息";
            this.ButtonStarted.UseVisualStyleBackColor = true;
            this.ButtonStarted.Click += new System.EventHandler(this.ButtonStarted_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(187, 23);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(64, 23);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "未啟動";
            // 
            // ReceiveExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.ButtonStarted);
            this.Controls.Add(this.receiveTextBox);
            this.Name = "ReceiveExample";
            this.Text = "RabbitMQ接收端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox receiveTextBox;
        private Button ButtonStarted;
        private Label statusLabel;
    }
}