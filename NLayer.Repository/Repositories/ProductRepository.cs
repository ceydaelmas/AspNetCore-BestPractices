using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)//contexti protected olarak belirlediğimiz için burdan erişebiliyoruz.
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Lazy Loading :efcoreda producta bağlı kategoriyi ihtiyaç olduğunda dahaa sonra çekersen bu lazy
            //Eager Loading : bir datayaı çekerken kategorilerin de gelmesini istedim. İlk productları çektiğim anda kategorileri de çekiyorum o yüzden Eager loading aşağıdaki örnek.
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
