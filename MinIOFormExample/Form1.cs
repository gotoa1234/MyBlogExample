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

        #region 1. MinIO 伺服器的資訊
        private readonly string _url = "192.168.51.188";
        private readonly int _port = 9005;
        private readonly string _accessKey = "uaub5kcqjFJovEpKZWr1";
        private readonly string _secretKey = "fIsxDIHae8Zx8Sa7wJt6KgN1hCXE490cLX1YOPRL";
        private readonly string _bucketName = "my-bucket";
        #endregion

        #region 2. 上傳檔案到 MinIO
        /// <summary>
        /// [Button] 瀏覽檔案
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
                    // 獲取選取的檔案路徑
                    textBox_filepath.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// [Button] 執行上傳
        /// </summary>        
        private void button_uploadFile_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {

                try
                {
                    // 使用建造者模式初始化 MinioClient
                    var minio = new MinioClient()
                                        .WithEndpoint(_url, _port)
                                        .WithCredentials(_accessKey, _secretKey)
                                        //.WithSSL() // 使用 HTTPS，才需要開啟這行
                                        .Build();

                    // 檢查 Bucket 是否存在                    
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

                    // 上傳一個檔案
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
                        await ShowMessageAsync("上傳成功!");
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

        #region 3. 下載檔案到本機
        /// <summary>
        /// [Button] 下載檔案-選擇存放資料夾
        /// </summary>        
        private void button_downloadPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "選擇下載檔案的資料夾";
                folderBrowserDialog.ShowNewFolderButton = true; // 是否允許建立新資料夾

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // 獲取選取的資料夾路徑
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    textBox_downloadPathFile.Text = selectedFolderPath; // 在文本框中顯示選取的資料夾路徑                  
                }
            }
        }

        /// <summary>
        /// [Button] 下載檔案
        /// </summary>  
        private void button_download_Click(object sender, EventArgs e)
        {
            _ = Task.Run(() => Working());

            async Task Working()
            {

                try
                {
                    // 使用建造者模式初始化 MinioClient
                    var minio = new MinioClient()
                                        .WithEndpoint(_url, _port)
                                        .WithCredentials(_accessKey, _secretKey)
                                        .Build();

                    // 下載檔案，取得名稱
                    string objectName = "test.txt";

                    // 下載該檔案
                    string downloadFilePath = textBox_downloadPathFile.Text + "\\" + objectName;
                    var getObjectArgs = new GetObjectArgs()
                                        .WithBucket(_bucketName)
                                        .WithObject(objectName)
                                        .WithFile(downloadFilePath);                    
                    await minio.GetObjectAsync(getObjectArgs);
                    await ShowMessageAsync($@"下載成功! 檔案路徑:{downloadFilePath}");

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

        #region 其他
        /// <summary>
        /// [Button] 關於按鈕
        /// </summary>        
        private void 關於ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 創建並顯示一個新的表單作為資訊框
            InfoForm infoForm = new InfoForm();
            infoForm.StartPosition = FormStartPosition.CenterParent;
            infoForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            infoForm.MaximizeBox = false;
            infoForm.MinimizeBox = false;
            infoForm.ShowDialog();
        }

        /// <summary>
        /// 關於 : 表單
        /// </summary>
        public class InfoForm : Form
        {
            public InfoForm()
            {
                this.Text = "關於";
                this.Size = new System.Drawing.Size(350, 170);

                Label newLabel = new Label();
                newLabel.Text = $@"                          【範例相關套件版本】" + Environment.NewLine;
                newLabel.Text += $@"     伺服器 MinIO 版本： RELEASE.2024-08-29T01-40-52Z" + Environment.NewLine;
                newLabel.Text += $@".Net MinIO SDK 版本：6.0.3" + Environment.NewLine;
                newLabel.Text += $@"             Visual Studio ： .Net Core 8.0" + Environment.NewLine;
                newLabel.Location = new System.Drawing.Point(10, 10);
                newLabel.AutoSize = true;
                this.Controls.Add(newLabel);

                // 作者
                LinkLabel linkLabelAuthor = new LinkLabel();
                linkLabelAuthor.Text = "作者Blog：https://gotoa1234.github.io/";
                linkLabelAuthor.Location = new System.Drawing.Point(10, 80);
                linkLabelAuthor.AutoSize = true;
                linkLabelAuthor.LinkClicked += (sender, e) =>
                {
                    // 開啟預設瀏覽器
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://gotoa1234.github.io/",
                        UseShellExecute = true
                    });
                };
                this.Controls.Add(linkLabelAuthor);

                // 贊助作者
                LinkLabel linkLabelSupport = new LinkLabel();
                linkLabelSupport.Text = "贊助作者：https://buymeacoffee.com/cap8825k";
                linkLabelSupport.Location = new System.Drawing.Point(10, 100);
                linkLabelSupport.AutoSize = true;
                linkLabelSupport.LinkClicked += (sender, e) =>
                {
                    // 開啟預設瀏覽器
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
        #endregion
    }
}
