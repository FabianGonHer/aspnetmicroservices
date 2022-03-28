using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(prop => prop.Name, name);

            return await _context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filterDefinition = Builders<Product>.Filter.Eq(prop => prop.Category, categoryName);

            return await _context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id.Equals(product.Id), product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string productId) 
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);

            var deletedResult = await _context.Products.DeleteOneAsync(filter);

            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }
    }
}
