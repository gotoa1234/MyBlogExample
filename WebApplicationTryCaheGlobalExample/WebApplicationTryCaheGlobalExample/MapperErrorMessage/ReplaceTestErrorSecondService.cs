using WebApplicationTryCaheGlobalExample.Common;

namespace WebApplicationTryCaheGlobalExample.Mapper
{

    public class ReplaceTestErrorSecondService : SingleTon< ReplaceTestErrorSecondService>
    {
        public string TestErrorSecond_MethodC(string errorMessage)
        {
            if (errorMessage == "執行錯誤")
            {
                return "自製錯誤訊息C";
            }
            return errorMessage;
        }

        public string TestErrorSecond_MethodD(string errorMessage)
        {
            if (errorMessage == "登入驗證失敗")
            {
                return "自製錯誤訊息D";
            }
            return errorMessage;
        }
    }
}