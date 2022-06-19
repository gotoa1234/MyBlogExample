using System;

namespace WebApplicationTryCaheGlobalExample.Base
{
    /// <summary>
    /// 例外覆寫
    /// </summary>
    public class MessageException : Exception
    {
        /// <param name="message">錯誤提示信息</param>
        public MessageException(string message) : base(message)
        {
        }
    }
}