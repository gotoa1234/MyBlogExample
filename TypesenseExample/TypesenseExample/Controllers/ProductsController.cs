using Microsoft.AspNetCore.Mvc;
using Typesense;
using TypesenseExample.Models;

namespace TypesenseExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ITypesenseClient _typesenseClient;
        private readonly ILogger<ProductsController> _logger;
        private const string CollectionName = "products";

        public ProductsController(
            ITypesenseClient typesenseClient,
            ILogger<ProductsController> logger)
        {
            _typesenseClient = typesenseClient;
            _logger = logger;
        }

        // 建立 Collection
        [HttpPost("setup")]
        public async Task<IActionResult> SetupCollection()
        {
            try
            {
                // 使用新的 Schema 建構方式
                var schema = new Schema(
                    name: CollectionName,
                    fields: new List<Field>
                    {
                        new Field("id", FieldType.String, false),
                        new Field("name", FieldType.String, false),
                        new Field("description", FieldType.String, false),
                        new Field("price", FieldType.Float, false),
                        new Field("category", FieldType.String, false, true), // facet = true
                        new Field("inStock", FieldType.Bool, false)
                    },
                    defaultSortingField: "price"
                );

                var result = await _typesenseClient.CreateCollection(schema);
                return Ok(new { message = "Collection created successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collection");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 刪除 Collection (用於重置)
        [HttpDelete("collection")]
        public async Task<IActionResult> DeleteCollection()
        {
            try
            {
                var result = await _typesenseClient.DeleteCollection(CollectionName);
                return Ok(new { message = "Collection deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting collection");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 新增單一商品
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                var result = await _typesenseClient.CreateDocument(CollectionName, product);
                return Ok(new { message = "Product added successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 批次匯入商品
        [HttpPost("import")]
        public async Task<IActionResult> ImportProducts([FromBody] List<Product> products)
        {
            try
            {
                // 第三個參數是 batch size
                var result = await _typesenseClient.ImportDocuments(CollectionName, products, 100);
                return Ok(new { message = "Products imported successfully", imported = products.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing products");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 搜尋商品
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string q = "*",
            [FromQuery] string? category = null,
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 20)
        {
            try
            {
                var searchParameters = new SearchParameters(
                    text: q,
                    queryBy: "name,description"
                )
                {
                    FilterBy = string.IsNullOrEmpty(category) ? null : $"category:={category}",
                    SortBy = "price:asc",
                    Page = page,
                    PerPage = perPage
                };

                var searchResult = await _typesenseClient.Search<Product>(CollectionName, searchParameters);

                var products = searchResult.Hits?.Select(h => h.Document).ToList() ?? new List<Product>();

                return Ok(new
                {
                    found = searchResult.Found,
                    page = searchResult.Page,
                    totalPages = (searchResult.Found + perPage - 1) / perPage,
                    products = products
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 取得單一商品
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var product = await _typesenseClient.RetrieveDocument<Product>(CollectionName, id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product");
                return NotFound(new { error = "Product not found" });
            }
        }

        // 更新商品
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            try
            {
                product.Id = id;
                var result = await _typesenseClient.UpdateDocument(CollectionName, id, product);
                return Ok(new { message = "Product updated successfully", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 刪除商品
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var result = await _typesenseClient.DeleteDocument<Product>(CollectionName, id);
                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 查看 Collection 資訊
        [HttpGet("collection-info")]
        public async Task<IActionResult> GetCollectionInfo()
        {
            try
            {
                var collection = await _typesenseClient.RetrieveCollection(CollectionName);
                return Ok(collection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collection info");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 查看所有 Collections
        [HttpGet("collections")]
        public async Task<IActionResult> GetAllCollections()
        {
            try
            {
                var collections = await _typesenseClient.RetrieveCollections();
                return Ok(collections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collections");
                return BadRequest(new { error = ex.Message });
            }
        }

        // 健康檢查
        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var health = await _typesenseClient.RetrieveHealth();
                return Ok(health);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking health");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
