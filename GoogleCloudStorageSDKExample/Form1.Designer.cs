namespace GoogleCloudStorageSDKExample
{
    partial class Form1
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
            label1 = new Label();
            GCS_browse_button = new Button();
            GCS_uploadFile_button = new Button();
            GCS_textBox_filepath = new TextBox();
            GCS_downloadPath_Button = new Button();
            GCS_download_Button = new Button();
            GCS_Download_FileNameTextBox = new TextBox();
            GCS_textBox_downloadPathFile = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 29);
            label1.Name = "label1";
            label1.Size = new Size(82, 23);
            label1.TabIndex = 0;
            label1.Text = "上傳檔案";
            // 
            // GCS_browse_button
            // 
            GCS_browse_button.Location = new Point(31, 68);
            GCS_browse_button.Name = "GCS_browse_button";
            GCS_browse_button.Size = new Size(112, 34);
            GCS_browse_button.TabIndex = 1;
            GCS_browse_button.Text = "選擇檔案";
            GCS_browse_button.UseVisualStyleBackColor = true;
            GCS_browse_button.Click += GCS_browse_button_Click;
            // 
            // GCS_uploadFile_button
            // 
            GCS_uploadFile_button.Location = new Point(169, 68);
            GCS_uploadFile_button.Name = "GCS_uploadFile_button";
            GCS_uploadFile_button.Size = new Size(112, 34);
            GCS_uploadFile_button.TabIndex = 2;
            GCS_uploadFile_button.Text = "上傳檔案";
            GCS_uploadFile_button.UseVisualStyleBackColor = true;
            GCS_uploadFile_button.Click += GCS_uploadFile_button_Click;
            // 
            // GCS_textBox_filepath
            // 
            GCS_textBox_filepath.Location = new Point(31, 128);
            GCS_textBox_filepath.Margin = new Padding(5);
            GCS_textBox_filepath.Name = "GCS_textBox_filepath";
            GCS_textBox_filepath.ReadOnly = true;
            GCS_textBox_filepath.Size = new Size(675, 30);
            GCS_textBox_filepath.TabIndex = 6;
            // 
            // GCS_downloadPath_Button
            // 
            GCS_downloadPath_Button.Location = new Point(31, 250);
            GCS_downloadPath_Button.Margin = new Padding(5);
            GCS_downloadPath_Button.Name = "GCS_downloadPath_Button";
            GCS_downloadPath_Button.Size = new Size(118, 35);
            GCS_downloadPath_Button.TabIndex = 11;
            GCS_downloadPath_Button.Text = "存放路徑";
            GCS_downloadPath_Button.UseVisualStyleBackColor = true;
            GCS_downloadPath_Button.Click += GCS_downloadPath_Button_Click;
            // 
            // GCS_download_Button
            // 
            GCS_download_Button.Location = new Point(169, 250);
            GCS_download_Button.Margin = new Padding(5);
            GCS_download_Button.Name = "GCS_download_Button";
            GCS_download_Button.Size = new Size(118, 35);
            GCS_download_Button.TabIndex = 12;
            GCS_download_Button.Text = "下載檔案";
            GCS_download_Button.UseVisualStyleBackColor = true;
            GCS_download_Button.Click += GCS_download_Button_Click;
            // 
            // GCS_Download_FileNameTextBox
            // 
            GCS_Download_FileNameTextBox.Location = new Point(295, 255);
            GCS_Download_FileNameTextBox.Name = "GCS_Download_FileNameTextBox";
            GCS_Download_FileNameTextBox.PlaceholderText = "輸入下載的檔案名稱";
            GCS_Download_FileNameTextBox.Size = new Size(411, 30);
            GCS_Download_FileNameTextBox.TabIndex = 13;
            // 
            // GCS_textBox_downloadPathFile
            // 
            GCS_textBox_downloadPathFile.Location = new Point(31, 304);
            GCS_textBox_downloadPathFile.Margin = new Padding(5);
            GCS_textBox_downloadPathFile.Name = "GCS_textBox_downloadPathFile";
            GCS_textBox_downloadPathFile.ReadOnly = true;
            GCS_textBox_downloadPathFile.Size = new Size(675, 30);
            GCS_textBox_downloadPathFile.TabIndex = 22;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(31, 210);
            label2.Name = "label2";
            label2.Size = new Size(82, 23);
            label2.TabIndex = 23;
            label2.Text = "下載檔案";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(GCS_textBox_downloadPathFile);
            Controls.Add(GCS_Download_FileNameTextBox);
            Controls.Add(GCS_download_Button);
            Controls.Add(GCS_downloadPath_Button);
            Controls.Add(GCS_textBox_filepath);
            Controls.Add(GCS_uploadFile_button);
            Controls.Add(GCS_browse_button);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Google Cloud Storage 上傳範例";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button GCS_browse_button;
        private Button GCS_uploadFile_button;
        private TextBox GCS_textBox_filepath;
        private Button GCS_downloadPath_Button;
        private Button GCS_download_Button;
        private TextBox GCS_Download_FileNameTextBox;
        private TextBox GCS_textBox_downloadPathFile;
        private Label label2;
    }
}
