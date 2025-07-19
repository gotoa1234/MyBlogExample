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
        /// 1. ��ܤW�Ǫ��ɮ�
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
                    // ���������ɮ׸��|
                    GCS_textBox_filepath.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// 2. ����W��
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

                    // �W�Ǥ@���ɮ�
                    string filePath = GCS_textBox_filepath.Text;
                    string objectName = Path.GetFileName(filePath);
                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        storage.UploadObject(_BucketName, objectName, null, fileStream);
                    }
                    await ShowMessageAsync("�W�Ǧ��\!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }

        /// <summary>
        /// 3. ��ܤU�����ɮצs���m
        /// </summary>
        private void GCS_downloadPath_Button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "��ܤU���ɮת���Ƨ�";
                folderBrowserDialog.ShowNewFolderButton = true; // �O�_���\�إ߷s��Ƨ�

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // ����������Ƨ����|
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    GCS_textBox_downloadPathFile.Text = selectedFolderPath; // �b�奻�ؤ���ܿ������Ƨ����|
                }
            }
        }

        /// <summary>
        /// 4. ����U��
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

                    // �U���@���ɮ�
                    string objectName = GCS_Download_FileNameTextBox.Text;
                    string downloadFilePath = GCS_textBox_downloadPathFile.Text + "\\" + objectName;
                    using (var outputFile = File.OpenWrite(downloadFilePath))
                    {
                        storage.DownloadObject(_BucketName, objectName, outputFile);
                    }
                    await ShowMessageAsync("�U�����\!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }


        /// <summary>
        /// �D�P�B��� MessageBox
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
