using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace GoogleCloudStorageSDKExample
{
    public partial class Form1 : Form
    {
        private readonly string _BucketName = $@"milkteagreenstorage";
        private readonly string _GCS_Crendential_Json = $@"
{{
  ""type"": ""service_account"",
  ""project_id"": """",
  ""private_key_id"": """",
  ""private_key"": """",
  ""client_email"": ""gcs-milkteagreen-uploader@my-project-32565-1696410670665.iam.gserviceaccount.com"",
  ""client_id"": ""106943431615570308192"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": """",
  ""universe_domain"": ""googleapis.com""
}}
";

        public Form1()
        {
            InitializeComponent();
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
                    var credential = GoogleCredential.FromJson(_GCS_Crendential_Json);
                    var storage = StorageClient.Create(credential);

                    // 上傳一個檔案
                    string filePath = GCS_textBox_filepath.Text;
                    string objectName = Path.GetFileName(filePath);
                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        storage.UploadObject(_BucketName, objectName, null, fileStream);
                    }
                    await ShowMessageAsync("上傳成功!");
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
                    var credential = GoogleCredential.FromJson(_GCS_Crendential_Json);
                    var storage = StorageClient.Create(credential);

                    // 下載一個檔案
                    string objectName = GCS_Download_FileNameTextBox.Text;
                    string downloadFilePath = GCS_textBox_downloadPathFile.Text + "\\" + objectName;
                    using (var outputFile = File.OpenWrite(downloadFilePath))
                    {
                        storage.DownloadObject(_BucketName, objectName, outputFile);
                    }
                    await ShowMessageAsync("下載成功!");
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
    }
}
