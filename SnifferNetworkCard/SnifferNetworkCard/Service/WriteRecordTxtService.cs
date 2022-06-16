using SnifferNetworkCard.Common;

namespace SnifferNetworkCard.Service
{
    /// <summary>
    /// 寫下紀錄存成.txt檔
    /// </summary>
    public class WriteRecordTxtService : Singleton<WriteRecordTxtService>
    {
        public void WriteText(string path = "", bool isappend = false, string fileName = "", string msg = "")
        {
            try
            {
                fileName = string.IsNullOrEmpty(fileName)
                           ? DateTime.Now.ToLongTimeString()
                           : fileName;
                fileName += ".txt";

                var usePath = !string.IsNullOrEmpty(path) && Directory.Exists(path)
                              ? $@"{path}\{fileName}"
                              : $@"{Environment.CurrentDirectory}\{fileName}";

                using (StreamWriter ss = new StreamWriter(usePath, isappend))
                {
                    ss.WriteLine($@" ------------  {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} ---------");
                    ss.WriteLine(msg);
                    ss.WriteLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
