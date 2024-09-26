namespace Example.Common.FakeDataBase.Model
{
    public class FileModel
    {
        /// <summary>
        /// 桶名稱
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 檔案清單
        /// </summary>
        public List<FileItem> Files { get; set; } = new List<FileItem>();
    }

    public class FileItem
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 檔案大小
        /// </summary>
        public ulong FileSize { get; set; }

        /// <summary>
        /// 轉成顯示格式
        /// </summary>
        public string ShowSize
        {
            get
            {
                if (this.FileSize < 1024)
                {
                    return this.FileSize + " Bytes";
                }
                else if (this.FileSize < 1024 * 1024)
                {
                    return (this.FileSize / 1024.0).ToString("F2") + " KB";
                }
                else if (this.FileSize < 1024 * 1024 * 1024)
                {
                    return (this.FileSize / (1024.0 * 1024)).ToString("F2") + " MB";
                }
                else
                {
                    return (this.FileSize / (1024.0 * 1024 * 1024)).ToString("F2") + " GB";
                }
            }
        }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 副檔名
        /// </summary>
        public string FileExtension { get; set; }
    }
}
