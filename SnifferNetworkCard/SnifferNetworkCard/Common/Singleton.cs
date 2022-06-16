namespace SnifferNetworkCard.Common
{
    /// <summary>
    /// 單例模式
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        public static Lazy<T> Instance { get; } = new Lazy<T>();
    }
}
