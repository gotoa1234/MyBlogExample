using System;

namespace WebApplicationTryCaheGlobalExample.Common
{
    public class SingleTon<T> where T : class, new()
    {
        public static Lazy<T> Instance { get; } = new Lazy<T>();
    }
}