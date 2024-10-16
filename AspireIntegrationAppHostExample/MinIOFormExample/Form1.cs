using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Diagnostics;

namespace MinIOFormExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 1. MinIO ���A������T
        private readonly string _url = "192.168.51.188";
        private readonly int _port = 9005;
        private readonly string _accessKey = "uaub5kcqjFJovEpKZWr1";
        private readonly string _secretKey = "fIsxDIHae8Zx8Sa7wJt6KgN1hCXE490cLX1YOPRL";
        private readonly string _bucketName = "my-bucket";
        #endregion

        #region 2. �W���ɮר� MinIO
        /// <summary>
        /// [Button] �s���ɮ�
        /// </summary>        
        private void button_browse_Click(object sender, EventArgs e)
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
                    textBox_filepath.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// [Button] ����W��
        /// </summary>        
        private void button_uploadFile_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {

                try
                {
                    // �ϥΫسy�̼Ҧ���l�� MinioClient
                    var minio = new MinioClient()
                                        .WithEndpoint(_url, _port)
                                        .WithCredentials(_accessKey, _secretKey)
                                        //.WithSSL() // �ϥ� HTTPS�A�~�ݭn�}�ҳo��
                                        .Build();

                    // �ˬd Bucket �O�_�s�b                    
                    var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
                    bool bucketExists = await minio.BucketExistsAsync(bucketExistsArgs);
                    if (!bucketExists)
                    {
                        var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
                        await minio.MakeBucketAsync(makeBucketArgs);
                        Console.WriteLine($"Bucket '{_bucketName}' created successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Bucket '{_bucketName}' already exists.");
                    }

                    // �W�Ǥ@���ɮ�
                    string filePath = textBox_filepath.Text;
                    string objectName = Path.GetFileName(filePath);
                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        var putObjectArgs = new PutObjectArgs()
                                            .WithBucket(_bucketName)
                                            .WithObject(objectName)
                                            .WithStreamData(fileStream)
                                            .WithObjectSize(fileStream.Length)
                                            .WithContentType("text/plain");
                        await minio.PutObjectAsync(putObjectArgs);
                        await ShowMessageAsync("�W�Ǧ��\!");
                    }
                }
                catch (MinioException e)
                {
                    Console.WriteLine($"MinIO Exception: {e}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }
        #endregion

        #region 3. �U���ɮר쥻��
        /// <summary>
        /// [Button] �U���ɮ�-��ܦs���Ƨ�
        /// </summary>        
        private void button_downloadPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "��ܤU���ɮת���Ƨ�";
                folderBrowserDialog.ShowNewFolderButton = true; // �O�_���\�إ߷s��Ƨ�

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // ����������Ƨ����|
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    textBox_downloadPathFile.Text = selectedFolderPath; // �b�奻�ؤ���ܿ������Ƨ����|                  
                }
            }
        }

        /// <summary>
        /// [Button] �U���ɮ�
        /// </summary>  
        private void button_download_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {

                try
                {
                    // �ϥΫسy�̼Ҧ���l�� MinioClient
                    var minio = new MinioClient()
                                        .WithEndpoint(_url, _port)
                                        .WithCredentials(_accessKey, _secretKey)
                                        .Build();

                    // �U���ɮסA���o�W��
                    string objectName = "test.txt";

                    // �U�����ɮ�
                    string downloadFilePath = textBox_downloadPathFile.Text + "\\" + objectName;
                    var getObjectArgs = new GetObjectArgs()
                                        .WithBucket(_bucketName)
                                        .WithObject(objectName)
                                        .WithFile(downloadFilePath);                    
                    await minio.GetObjectAsync(getObjectArgs);
                    await ShowMessageAsync($@"�U�����\! �ɮ׸��|:{downloadFilePath}");

                }
                catch (MinioException e)
                {
                    Console.WriteLine($"MinIO Exception: {e}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
        }
        #endregion

        #region ��L
        /// <summary>
        /// [Button] ������s
        /// </summary>        
        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �Ыب���ܤ@�ӷs�����@����T��
            InfoForm infoForm = new InfoForm();
            infoForm.StartPosition = FormStartPosition.CenterParent;
            infoForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            infoForm.MaximizeBox = false;
            infoForm.MinimizeBox = false;
            infoForm.ShowDialog();
        }

        /// <summary>
        /// ���� : ���
        /// </summary>
        public class InfoForm : Form
        {
            public InfoForm()
            {
                this.Text = "����";
                this.Size = new System.Drawing.Size(350, 170);

                Label newLabel = new Label();
                newLabel.Text = $@"                          �i�d�Ҭ����M�󪩥��j" + Environment.NewLine;
                newLabel.Text += $@"     ���A�� MinIO �����G RELEASE.2024-08-29T01-40-52Z" + Environment.NewLine;
                newLabel.Text += $@".Net MinIO SDK �����G6.0.3" + Environment.NewLine;
                newLabel.Text += $@"             Visual Studio �G .Net Core 8.0" + Environment.NewLine;
                newLabel.Location = new System.Drawing.Point(10, 10);
                newLabel.AutoSize = true;
                this.Controls.Add(newLabel);

                // �@��
                LinkLabel linkLabelAuthor = new LinkLabel();
                linkLabelAuthor.Text = "�@��Blog�Ghttps://gotoa1234.github.io/";
                linkLabelAuthor.Location = new System.Drawing.Point(10, 80);
                linkLabelAuthor.AutoSize = true;
                linkLabelAuthor.LinkClicked += (sender, e) =>
                {
                    // �}�ҹw�]�s����
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://gotoa1234.github.io/",
                        UseShellExecute = true
                    });
                };
                this.Controls.Add(linkLabelAuthor);

                // �٧U�@��
                LinkLabel linkLabelSupport = new LinkLabel();
                linkLabelSupport.Text = "�٧U�@�̡Ghttps://buymeacoffee.com/cap8825k";
                linkLabelSupport.Location = new System.Drawing.Point(10, 100);
                linkLabelSupport.AutoSize = true;
                linkLabelSupport.LinkClicked += (sender, e) =>
                {
                    // �}�ҹw�]�s����
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://buymeacoffee.com/cap8825k",
                        UseShellExecute = true
                    });
                };
                this.Controls.Add(linkLabelSupport);
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
        #endregion
    }
}
