using System;
using WebApplicationTryCaheGlobalExample.Base;
using WebApplicationTryCaheGlobalExample.Common;

namespace WebApplicationTryCaheGlobalExample.Service
{
    public class TestErrorFirstService : SingleTon<TestErrorFirstService>
    {
        public void TestErrorFirst_MethodA()
        {
            Validate();

            void Validate()
            {
                throw new MessageException($@"執行函式錯誤");
            }
        }

        public void TestErrorFirst_MethodB()
        {
            LoadDataBase();

            void LoadDataBase()
            {
                throw new MessageException($@"讀取資料庫位址錯誤");
            }
        }

    }
}