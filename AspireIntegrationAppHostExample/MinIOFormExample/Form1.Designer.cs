namespace MinIOFormExample
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
            button_browse = new Button();
            openFileDialog = new OpenFileDialog();
            textBox_filepath = new TextBox();
            button_uploadFile = new Button();
            button_download = new Button();
            tabControl1 = new TabControl();
            tabPage_upload = new TabPage();
            tabPage_download = new TabPage();
            label1 = new Label();
            textBox_downloadPathFile = new TextBox();
            button_downloadPath = new Button();
            menuStrip1 = new MenuStrip();
            關於ToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage_upload.SuspendLayout();
            tabPage_download.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button_browse
            // 
            button_browse.Location = new Point(6, 6);
            button_browse.Name = "button_browse";
            button_browse.Size = new Size(75, 23);
            button_browse.TabIndex = 0;
            button_browse.Text = "選擇檔案";
            button_browse.UseVisualStyleBackColor = true;
            button_browse.Click += button_browse_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // textBox_filepath
            // 
            textBox_filepath.Location = new Point(6, 35);
            textBox_filepath.Name = "textBox_filepath";
            textBox_filepath.ReadOnly = true;
            textBox_filepath.Size = new Size(431, 23);
            textBox_filepath.TabIndex = 1;
            // 
            // button_uploadFile
            // 
            button_uploadFile.Location = new Point(87, 6);
            button_uploadFile.Name = "button_uploadFile";
            button_uploadFile.Size = new Size(75, 23);
            button_uploadFile.TabIndex = 2;
            button_uploadFile.Text = "上傳檔案";
            button_uploadFile.UseVisualStyleBackColor = true;
            button_uploadFile.Click += button_uploadFile_Click;
            // 
            // button_download
            // 
            button_download.Location = new Point(87, 6);
            button_download.Name = "button_download";
            button_download.Size = new Size(75, 23);
            button_download.TabIndex = 3;
            button_download.Text = "下載檔案";
            button_download.UseVisualStyleBackColor = true;
            button_download.Click += button_download_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage_upload);
            tabControl1.Controls.Add(tabPage_download);
            tabControl1.Location = new Point(12, 27);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(465, 98);
            tabControl1.TabIndex = 5;
            // 
            // tabPage_upload
            // 
            tabPage_upload.Controls.Add(textBox_filepath);
            tabPage_upload.Controls.Add(button_uploadFile);
            tabPage_upload.Controls.Add(button_browse);
            tabPage_upload.Location = new Point(4, 24);
            tabPage_upload.Name = "tabPage_upload";
            tabPage_upload.Padding = new Padding(3);
            tabPage_upload.Size = new Size(457, 70);
            tabPage_upload.TabIndex = 0;
            tabPage_upload.Text = "上傳檔案";
            tabPage_upload.UseVisualStyleBackColor = true;
            // 
            // tabPage_download
            // 
            tabPage_download.Controls.Add(label1);
            tabPage_download.Controls.Add(button_download);
            tabPage_download.Controls.Add(textBox_downloadPathFile);
            tabPage_download.Controls.Add(button_downloadPath);
            tabPage_download.Location = new Point(4, 24);
            tabPage_download.Name = "tabPage_download";
            tabPage_download.Padding = new Padding(3);
            tabPage_download.Size = new Size(457, 70);
            tabPage_download.TabIndex = 1;
            tabPage_download.Text = "下載檔案";
            tabPage_download.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(174, 11);
            label1.Name = "label1";
            label1.Size = new Size(107, 15);
            label1.TabIndex = 8;
            label1.Text = "範例檔案： test.txt";
            // 
            // textBox_downloadPathFile
            // 
            textBox_downloadPathFile.Location = new Point(6, 35);
            textBox_downloadPathFile.Name = "textBox_downloadPathFile";
            textBox_downloadPathFile.ReadOnly = true;
            textBox_downloadPathFile.Size = new Size(431, 23);
            textBox_downloadPathFile.TabIndex = 7;
            // 
            // button_downloadPath
            // 
            button_downloadPath.Location = new Point(6, 6);
            button_downloadPath.Name = "button_downloadPath";
            button_downloadPath.Size = new Size(75, 23);
            button_downloadPath.TabIndex = 6;
            button_downloadPath.Text = "存放路徑";
            button_downloadPath.UseVisualStyleBackColor = true;
            button_downloadPath.Click += button_downloadPath_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 關於ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(483, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // 關於ToolStripMenuItem
            // 
            關於ToolStripMenuItem.Name = "關於ToolStripMenuItem";
            關於ToolStripMenuItem.Size = new Size(43, 20);
            關於ToolStripMenuItem.Text = "關於";
            關於ToolStripMenuItem.Click += 關於ToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(483, 137);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterParent;
            Text = "MinIO WinForm範例代碼";
            tabControl1.ResumeLayout(false);
            tabPage_upload.ResumeLayout(false);
            tabPage_upload.PerformLayout();
            tabPage_download.ResumeLayout(false);
            tabPage_download.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_browse;
        private OpenFileDialog openFileDialog;
        private TextBox textBox_filepath;
        private Button button_uploadFile;
        private Button button_download;
        private TabControl tabControl1;
        private TabPage tabPage_upload;
        private TabPage tabPage_download;
        private TextBox textBox_downloadPathFile;
        private Button button_downloadPath;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 關於ToolStripMenuItem;
        private Label label1;
    }
}
