using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace GetClassAndPropertyDescriptionExample
{
    public partial class GetClassAndPropertyDescriptionExampleForm : Form
    {
        private readonly string _xmlPath = $@"{Directory.GetCurrentDirectory()}\{Assembly.GetExecutingAssembly().GetName().Name}.xml";

        /// <summary>
        /// Form 建構式
        /// </summary>
        public GetClassAndPropertyDescriptionExampleForm()
        {
            InitializeComponent();
        }

        #region 【1】查詢
       
        /// <summary>
        /// 【1】查詢按鈕
        /// </summary>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            var inputClassName = InputCllassName_textBox.Text;
            try
            {
                if (!IsClassNameExsis(inputClassName))
                {
                    MessageBox.Show("className無效，請輸入完整的 命名空間 + ClassName !");
                    return;
                }
                GetSpecifyClassNameInfomation(inputClassName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 【1】 檢核ClassName是否存在
        /// </summary>
        private bool IsClassNameExsis(string className)
        {
            if (!IsExsis() || string.IsNullOrEmpty(className))
            {
                return false;
            }
            var _docuDoc = new System.Xml.XmlDocument();
            _docuDoc.Load(_xmlPath);

            var xmlDocuOfMethod = _docuDoc.SelectNodes(
                "//member[starts-with(@name, '" + $@"P:{className}" + "')]");

            if (xmlDocuOfMethod == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 【1】 取得特定的Class裡面的PropertyName 與 Summary
        /// </summary>
        private void GetSpecifyClassNameInfomation(string className)
        {
            var _docuDoc = new System.Xml.XmlDocument();
            _docuDoc.Load(_xmlPath);

            var xmlDocuOfMethod = _docuDoc.SelectNodes(
                "//member[starts-with(@name, '" + $@"P:{className}" + "')]");
            if (xmlDocuOfMethod != null)
            {
                var dictionary = new Dictionary<string, string>();
                Regex filter = new Regex(@"\..*$");
                for (int i = 0; i < xmlDocuOfMethod.Count; i++)
                {
                    var nameTag = xmlDocuOfMethod[i].Attributes["name"].Value.Replace(className, "");
                    var name = filter.Match(nameTag).Value.Replace(".", ""); ;

                    var summary = xmlDocuOfMethod[i].InnerText;
                    dictionary.Add(name, summary);
                }

                var resultString = new StringBuilder();
                foreach (var dicItem in dictionary)
                {
                    resultString.AppendLine($"PropertyName：{dicItem.Key}");
                    resultString.AppendLine($"Summary：{dicItem.Value.Replace("\r\n", "").Trim()}");
                    resultString.AppendLine();
                }
                ConverttextBoxMessage.Text = resultString.ToString();
            }
        }

        #endregion

        #region 【2】轉換對應物件按鈕

        /// <summary>
        /// 【2】 轉換對應物件按鈕
        /// </summary>
        private void ConvertClass_button_Click(object sender, EventArgs e)
        {
            var inputClassName = InputCllassName_textBox.Text;
            try
            {
                if (!IsClassNameExsis(inputClassName))
                {
                    MessageBox.Show("className無效，請輸入完整的 命名空間 + ClassName !");
                    return;
                }
                GetSpecifyClassNameConvertObjecString(inputClassName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 【2】 取得客製的Mapping 代碼
        /// </summary>
        private void GetSpecifyClassNameConvertObjecString(string className)
        {
            var _docuDoc = new System.Xml.XmlDocument();
            _docuDoc.Load(_xmlPath);

            var xmlDocuOfMethod = _docuDoc.SelectNodes(
                "//member[starts-with(@name, '" + $@"P:{className}" + "')]");
            if (xmlDocuOfMethod != null)
            {
                var dictionary = new Dictionary<string, string>();
                Regex filter = new Regex(@"\..*$");
                for (int i = 0; i < xmlDocuOfMethod.Count; i++)
                {
                    var nameTag = xmlDocuOfMethod[i].Attributes["name"].Value.Replace(className, "");
                    var name = filter.Match(nameTag).Value.Replace(".", ""); ;

                    var summary = xmlDocuOfMethod[i].InnerText;
                    dictionary.Add(name, summary);
                }

                var resultString = new StringBuilder();
                foreach (var dicItem in dictionary)
                {
                    string path = "P:" + className + "." + dicItem.Key;
                    XmlNode findXmlDocuOfMethod = _docuDoc.SelectSingleNode(
                        "//member[starts-with(@name, '" + path + "')]");
                    var filterWord = new Regex(@"[A-Za-z0-9]+");
                    var match = filterWord.Match(findXmlDocuOfMethod.InnerText.Replace("\r\n", ""));
                    //match.Groups[0].Value  取得原名稱
                    //prop.Name 取得新名稱
                    if (match.Success)
                    {
                        resultString.AppendLine($@"{dicItem.Key} = input.{match.Groups[0].Value},");
                    }
                }
                ConverttextBoxMessage.Text = resultString.ToString();

            }
        }

        #endregion

        #region 【3】取得強型別類型的物件內容方法

        /// <summary>
        ///【3】 取得強型別類型的物件內容方法
        /// </summary>
        private void StronglyTypeClassConvertbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsExsis())
                {
                    MessageBox.Show("無XML檔案，無法解析!");
                }
                LoadXml<GetClassAndPropertyDescriptionExample.ViewModel.Changed.LoginViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        ///【3】 泛型物件轉換
        /// </summary>
        private void LoadXml<T>()
        {
            var properties = typeof(T).GetProperties();
            var _docuDoc = new System.Xml.XmlDocument();
            _docuDoc.Load(_xmlPath);

            var resultString = string.Empty;
            foreach (var prop in properties)
            {
                var fullname = prop.DeclaringType.FullName.Replace('+', '.');
                string path = "P:" + fullname + "." + prop.Name;

                XmlNode xmlDocuOfMethod = _docuDoc.SelectSingleNode(
                    "//member[starts-with(@name, '" + path + "')]");
                Regex filter = new Regex(@"([A-Za-z]+)");
                var match = filter.Match(xmlDocuOfMethod.InnerText.Replace("\r\n", ""));
                //match.Groups[0].Value  取得原名稱
                //prop.Name 取得新名稱
                if (match.Success)
                {
                    resultString += $@"{prop.Name} = input.{match.Groups[0].Value}," + Environment.NewLine;
                }
                ConverttextBoxMessage.Text = resultString; 
            }
        }

        #endregion

        /// <summary>
        /// 共用方法，檢核XML檔案是否存在
        /// </summary>
        private bool IsExsis()
        {
            if (File.Exists(_xmlPath))
            {
                return true;
            }
            return false;
        }


    }
}