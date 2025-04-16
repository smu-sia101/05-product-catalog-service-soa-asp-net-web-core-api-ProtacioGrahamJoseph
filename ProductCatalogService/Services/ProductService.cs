using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCatalogService.Models;

namespace ProductCatalogService.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<ProductCatalogDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _products = mongoDatabase.GetCollection<Product>(settings.Value.ProductsCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _products.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) =>
            await _products.InsertOneAsync(product);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _products.ReplaceOneAsync(p => p.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _products.DeleteOneAsync(p => p.Id == id);
    }
}
