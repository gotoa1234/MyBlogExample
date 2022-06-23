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
        /// Form �غc��
        /// </summary>
        public GetClassAndPropertyDescriptionExampleForm()
        {
            InitializeComponent();
        }

        #region �i1�j�d��
       
        /// <summary>
        /// �i1�j�d�߫��s
        /// </summary>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            var inputClassName = InputCllassName_textBox.Text;
            try
            {
                if (!IsClassNameExsis(inputClassName))
                {
                    MessageBox.Show("className�L�ġA�п�J���㪺 �R�W�Ŷ� + ClassName !");
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
        /// �i1�j �ˮ�ClassName�O�_�s�b
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
        /// �i1�j ���o�S�w��Class�̭���PropertyName �P Summary
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
                    resultString.AppendLine($"PropertyName�G{dicItem.Key}");
                    resultString.AppendLine($"Summary�G{dicItem.Value.Replace("\r\n", "").Trim()}");
                    resultString.AppendLine();
                }
                ConverttextBoxMessage.Text = resultString.ToString();
            }
        }

        #endregion

        #region �i2�j�ഫ����������s

        /// <summary>
        /// �i2�j �ഫ����������s
        /// </summary>
        private void ConvertClass_button_Click(object sender, EventArgs e)
        {
            var inputClassName = InputCllassName_textBox.Text;
            try
            {
                if (!IsClassNameExsis(inputClassName))
                {
                    MessageBox.Show("className�L�ġA�п�J���㪺 �R�W�Ŷ� + ClassName !");
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
        /// �i2�j ���o�Ȼs��Mapping �N�X
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
                    //match.Groups[0].Value  ���o��W��
                    //prop.Name ���o�s�W��
                    if (match.Success)
                    {
                        resultString.AppendLine($@"{dicItem.Key} = input.{match.Groups[0].Value},");
                    }
                }
                ConverttextBoxMessage.Text = resultString.ToString();

            }
        }

        #endregion

        #region �i3�j���o�j���O���������󤺮e��k

        /// <summary>
        ///�i3�j ���o�j���O���������󤺮e��k
        /// </summary>
        private void StronglyTypeClassConvertbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsExsis())
                {
                    MessageBox.Show("�LXML�ɮסA�L�k�ѪR!");
                }
                LoadXml<GetClassAndPropertyDescriptionExample.ViewModel.Changed.LoginViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        ///�i3�j �x�������ഫ
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
                //match.Groups[0].Value  ���o��W��
                //prop.Name ���o�s�W��
                if (match.Success)
                {
                    resultString += $@"{prop.Name} = input.{match.Groups[0].Value}," + Environment.NewLine;
                }
                ConverttextBoxMessage.Text = resultString; 
            }
        }

        #endregion

        /// <summary>
        /// �@�Τ�k�A�ˮ�XML�ɮ׬O�_�s�b
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