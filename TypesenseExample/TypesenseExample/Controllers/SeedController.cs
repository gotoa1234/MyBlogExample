using Microsoft.AspNetCore.Mvc;
using Typesense;
using TypesenseExample.Models;

namespace TypesenseExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ITypesenseClient _typesenseClient;
        private const string CollectionName = "products";

        public SeedController(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
        }

        [HttpPost("init")]
        public async Task<IActionResult> InitializeData()
        {
            try
            {
                // 1. 嘗試刪除舊的 Collection (如果存在)
                try
                {
                    await _typesenseClient.DeleteCollection(CollectionName);
                }
                catch
                {
                    // Collection 不存在,忽略錯誤
                }

                // 2. 建立新的 Collection
                var schema = new Schema(
                    name: CollectionName,
                    fields: new List<Field>
                    {
                        new Field("id", FieldType.String, false),
                        new Field("name", FieldType.String, false),
                        new Field("description", FieldType.String, false),
                        new Field("price", FieldType.Float, false),
                        new Field("category", FieldType.String, false, true),
                        new Field("inStock", FieldType.Bool, false)
                    },
                    defaultSortingField: "price"
                );

                await _typesenseClient.CreateCollection(schema);

                // 3. 準備測試資料
                var products = new List<Product>
                {
                    new Product
                    {
                        Id = "1",
                        Name = "iPhone 15 Pro",
                        Description = "最新的 Apple 旗艦手機，搭載 A17 Pro 晶片",
                        Price = 35900,
                        Category = "手機",
                        InStock = true
                    },
                    new Product
                    {
                        Id = "2",
                        Name = "MacBook Pro 14",
                        Description = "M3 Pro 晶片筆記型電腦，效能強大",
                        Price = 62900,
                        Category = "筆電",
                        InStock = true
                    },
                    new Product
                    {
                        Id = "3",
                        Name = "AirPods Pro 2",
                        Description = "主動降噪無線耳機，音質優異",
                        Price = 7490,
                        Category = "耳機",
                        InStock = false
                    },
                    new Product
                    {
                        Id = "4",
                        Name = "iPad Air",
                        Description = "輕薄強大的平板電腦，適合工作娛樂",
                        Price = 18900,
                        Category = "平板",
                        InStock = true
                    },
                    new Product
                    {
                        Id = "5",
                        Name = "Apple Watch Series 9",
                        Description = "智慧手錶，健康追蹤功能完善",
                        Price = 12900,
                        Category = "穿戴裝置",
                        InStock = true
                    }
                };

                // 4. 批次匯入資料
                await _typesenseClient.ImportDocuments(CollectionName, products, 100);

                return Ok(new
                {
                    message = "資料初始化成功!",
                    collectionName = CollectionName,
                    productsCount = products.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
