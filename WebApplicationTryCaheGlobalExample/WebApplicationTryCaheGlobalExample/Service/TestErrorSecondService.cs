using WebApplicationTryCaheGlobalExample.Base;
using WebApplicationTryCaheGlobalExample.Common;

namespace WebApplicationTryCaheGlobalExample.Service
{
    public class TestErrorSecondService : SingleTon<TestErrorSecondService>
    {
        public void TestErrorSecond_MethodC()
        {
            Excute();

            void Excute()
            {
                throw new MessageException($@"執行錯誤");
            }
        }

        public void TestErrorSecond_MethodD()
        {
            LoginValidate();

            void LoginValidate()
            {
                throw new MessageException($@"登入驗證失敗");
            }
        }
    }
}