using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace GoogleCloudStorageSupportS3APIExample
{
    public partial class Form1 : Form
    {
        private ServerEnum currentServer = ServerEnum.GoogleCloudStorage;
        private string _BucketName = $@"";
        private string _AccessKey = "";
        private string _SecretKey = "";
        private AmazonS3Config _Config;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��l�Ƹ��J
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            GCS_textBox_filepath.Text = "D:\\test.txt";
            GCSCredentialbutton_Click(new object(), new EventArgs());
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
                    //2-1. �W�Ǯ� AWS S3 SDK ��� Minio / GCS �U�����ݮe�ʪ��t��
                    if (currentServer == ServerEnum.GoogleCloudStorage)
                    {
                        // 2-2. GCS �ݭn�� PutObjectRequest (Minio�]�i�䴩)
                        await GoogleCloudStorageUpload();
                    }
                    else if (currentServer == ServerEnum.Minio)
                    {
                        // 2-3. Minio Server �P AWS S3 SDK ���׭ݮe�i�ϥΧ�s�� TransferUtility
                        await MinioServerUpload();
                    }                      
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }

            // AWS S3 SDK �W�Ǩ� Minio Server
            async Task MinioServerUpload()
            {
                using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                {
                    var transferUtility = new TransferUtility(s3Client);

                    // �W���ɮ׸�T
                    string filePath = GCS_textBox_filepath.Text;
                    string objectName = Path.GetFileName(filePath);

                    // Minio ���׭ݮe S3 API �A�i�ϥγ̷s�� transferUtility ��k
                    await transferUtility.UploadAsync(filePath, _BucketName, objectName);
                    await ShowMessageAsync("�W�Ǧ��\!");
                }
            }

            // AWS S3 SDK �W�Ǩ� GCS Server
            async Task GoogleCloudStorageUpload()
            {
                using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                {
                    // �W���ɮ׸�T
                    string filePath = GCS_textBox_filepath.Text;
                    string objectName = Path.GetFileName(filePath);

                    // GCS �ݩ�ݮe S3 API �A�]�������� PutObjectRequest
                    var request = new PutObjectRequest
                    {
                        BucketName = _BucketName,
                        Key = objectName,
                        FilePath = filePath,
                        ContentType = "application/octet-stream",
                        UseChunkEncoding = false
                    };
                    var response = await s3Client.PutObjectAsync(request);
                    await ShowMessageAsync("�W�Ǧ��\!");
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
                    //4-1. �W�Ǯ� AWS S3 SDK ��� Minio / GCS �U�����ݮe�ʪ��t��
                    if (currentServer == ServerEnum.GoogleCloudStorage)
                    {
                        // 4-2. GCS �ݭn�� GetObjectRequest (Minio�]�i�䴩)
                        await GoogleCloudStorageDownload();
                    }
                    else if (currentServer == ServerEnum.Minio)
                    {
                        // 4-3. Minio Server �P AWS S3 SDK ���׭ݮe�i�ϥΧ�s�� TransferUtility
                        await MinioServerDownload();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }

            // AWS S3 SDK �q Minio Server �U���ɮ�
            async Task MinioServerDownload()
            {
                using (var s3Client = new AmazonS3Client(_AccessKey, _SecretKey, _Config))
                {
                    string objectName = GCS_Download_FileNameTextBox.Text;
                    string downloadFilePath = Path.Combine(GCS_textBox_downloadPathFile.Text, objectName);

                    // �i�Χ��²�B���ժ� TransferUtility 
                    var transferUtility = new TransferUtility(s3Client);                    
                    await transferUtility.DownloadAsync(downloadFilePath, _BucketName, objectName);

                    await ShowMessageAsync("�U�����\!");
                }
            }

            // AWS S3 SDK �q GCS Server �U���ɮ� (Minio ��䴩����k)
            async Task GoogleCloudStorageDownload()
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

                    await ShowMessageAsync("�U�����\!");
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

        /// <summary>
        /// �ϥ� GCS ����
        /// </summary>
        private void GCSCredentialbutton_Click(object sender, EventArgs e)
        {
            var useMessage = "GCS";
            currentServer = ServerEnum.GoogleCloudStorage;
            _BucketName = $@"";
            _AccessKey = "";
            _SecretKey = "";

            _Config = new AmazonS3Config
            {                
                ServiceURL = "https://storage.googleapis.com",
                ForcePathStyle = true,
            };
            
            UsedCredentialLable.Text = $@"��e�ϥξ��ҡG{useMessage}";
        }

        /// <summary>
        /// �ϥ� Minio ����
        /// </summary>
        private void MinioCredentialbutton_Click(object sender, EventArgs e)
        {
            var useMessage = "Minio";
            currentServer = ServerEnum.Minio;
            _BucketName = $@"";
            _AccessKey = "";
            _SecretKey = "";
            _Config = new AmazonS3Config
            {
                ServiceURL = "http://stg.minio.mg",
                ForcePathStyle = true                
            };
            UsedCredentialLable.Text = $@"��e�ϥξ��ҡG{useMessage}";
        }        

    }
}
