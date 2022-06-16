namespace SnifferNetworkCard.Common
{
    public static class Util
    {
        /// <summary>
        /// 取得泛型Class T的 指定propertyName 的Description
        /// </summary>
        public static string GetDescription<T>(string propertyName) where T : class
        {
            var getType = typeof(T);
            if (getType != null && 
                !string.IsNullOrEmpty(propertyName))
            {
                var attribData = getType?.GetProperty(propertyName)
                 .GetCustomAttributesData()
                 .Where(item => item.AttributeType.Name == nameof(System.ComponentModel.DescriptionAttribute))
                 .FirstOrDefault();
                if (attribData != null && 
                    attribData?.ConstructorArguments.Count > 0)
                {
                    return attribData.ConstructorArguments[0].Value.ToString();
                }
            }
            return string.Empty;
        }
    }
}
