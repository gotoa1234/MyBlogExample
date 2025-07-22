using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace GoogleCloudStorageSupportS3APIExample
{
    public partial class Form1 : Form
    {
        private string _BucketName = $@"milkteagreenstorage";
        private string _AccessKey = "";
        private string _SecretKey = "";
        private AmazonS3Config _Config;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化載入
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            GCSCredentialbutton_Click(new object(), new EventArgs());
        }

        /// <summary>
        /// 1. 選擇上傳的檔案
        /// </summary>
        private void GCS_browse_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 獲取選取的檔案路徑
                    GCS_textBox_filepath.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// 2. 執行上傳
        /// </summary>
        private void GCS_uploadFile_button_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {

                try
                {
                    using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                    {
                        var transferUtility = new TransferUtility(s3Client);

                        // 上傳檔案
                        string filePath = GCS_textBox_filepath.Text;
                        string objectName = Path.GetFileName(filePath);

                        await transferUtility.UploadAsync(filePath, _BucketName, objectName);
                        await ShowMessageAsync("上傳成功!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }

        /// <summary>
        /// 3. 選擇下載的檔案存放位置
        /// </summary>
        private void GCS_downloadPath_Button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "選擇下載檔案的資料夾";
                folderBrowserDialog.ShowNewFolderButton = true; // 是否允許建立新資料夾

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // 獲取選取的資料夾路徑
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    GCS_textBox_downloadPathFile.Text = selectedFolderPath; // 在文本框中顯示選取的資料夾路徑
                }
            }

        }

        /// <summary>
        /// 4. 執行下載
        /// </summary>
        private void GCS_download_Button_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {
                try
                {
                    using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                    {
                        string objectName = GCS_Download_FileNameTextBox.Text;
                        string downloadFilePath = Path.Combine(GCS_textBox_downloadPathFile.Text, objectName);

                        var request = new GetObjectRequest
                        {
                            BucketName = _BucketName,
                            Key = objectName
                        };

                        using var response = await s3Client.GetObjectAsync(request);
                        await using var responseStream = response.ResponseStream;
                        await using var fileStream = File.Create(downloadFilePath);
                        await responseStream.CopyToAsync(fileStream);

                        await ShowMessageAsync("下載成功!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }


        /// <summary>
        /// 非同步顯示 MessageBox
        /// </summary>        
        private Task ShowMessageAsync(string message)
        {
            return Task.Run(() =>
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => MessageBox.Show(message)));
                }
                else
                {
                    MessageBox.Show(message);
                }
            });
        }

        /// <summary>
        /// 使用 GCS 憑證
        /// </summary>
        private void GCSCredentialbutton_Click(object sender, EventArgs e)
        {
            var useMessage = "GCS";
            _BucketName = $@"milkteagreenstorage";
            _AccessKey = "";
            _SecretKey = "";

            _Config = new AmazonS3Config
            {
                ServiceURL = "https://storage.googleapis.com",
                ForcePathStyle = true // GCS 需要 Path-style URL
            };
            UsedCredentialLable.Text = $@"當前使用憑證：{useMessage}";
        }

        /// <summary>
        /// 使用 Minio 憑證
        /// </summary>
        private void MinioCredentialbutton_Click(object sender, EventArgs e)
        {
            var useMessage = "Minio";
            _BucketName = $@"louistest";
            _AccessKey = "Qr0NvVfcdhDIOyG4DnAf";
            _SecretKey = "ajb3rh5JtnrvAOQTVOOd21r6hDRgA5krZqg3zrjv";
            _Config = new AmazonS3Config
            {
                ServiceURL = "http://stg.minio.mg",
                ForcePathStyle = true                
            };
            UsedCredentialLable.Text = $@"當前使用憑證：{useMessage}";
        }
    }
}
