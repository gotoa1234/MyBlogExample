using WebApplicationTryCaheGlobalExample.Common;

namespace WebApplicationTryCaheGlobalExample.MapperErrorMessage
{
    public class ReplaceTestErrorFirstService : SingleTon<ReplaceTestErrorFirstService>
    {
        public string TestErrorSecond_MethodA(string errorMessage)
        {
            if (errorMessage == "執行函式錯誤")
            {
                return "自製錯誤訊息A";
            }
            return errorMessage;
        }

        public string TestErrorSecond_MethodB(string errorMessage)
        {
            if (errorMessage == "讀取資料庫位址錯誤")
            {
                return "自製錯誤訊息B";
            }
            return errorMessage;
        }
    }
}