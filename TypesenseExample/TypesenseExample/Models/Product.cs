using System.Text.Json.Serialization;

namespace TypesenseExample.Models
{
    /// <summary>
    /// Product 資料模型
    /// </summary>
    public class Product
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public float Price { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("inStock")]
        public bool InStock { get; set; } = true;
    }

}
