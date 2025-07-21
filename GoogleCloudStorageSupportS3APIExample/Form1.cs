using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace GoogleCloudStorageSupportS3APIExample
{
    public partial class Form1 : Form
    {
        private readonly string _BucketName = $@"milkteagreenstorage";
        private readonly string _AccessKey = "";
        private readonly string _SecretKey = "";
        private readonly AmazonS3Config _Config;

        public Form1()
        {
            _Config = new AmazonS3Config
            {
                ServiceURL = "https://storage.googleapis.com",
                ForcePathStyle = true // GCS �ݭn Path-style URL
            };

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                    using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                    {
                        var transferUtility = new TransferUtility(s3Client);

                        // �W���ɮ�
                        string filePath = GCS_textBox_filepath.Text;
                        string objectName = Path.GetFileName(filePath);

                        await transferUtility.UploadAsync(filePath, _BucketName, objectName);
                        await ShowMessageAsync("�W�Ǧ��\!");
                    }
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
                    using( var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
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

                        await ShowMessageAsync("�U�����\!");
                    }
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
