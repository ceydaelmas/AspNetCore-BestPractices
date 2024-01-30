using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    //var olan yapıyı bozmadan bunu implemet edeceğim. Apı katmanı IPRoductService kullanıyor ben de onu implement edeceğim. Buna Proxy ya da Decorative Design Pattern denebilir.
    //Değişime kapalı ama gelişime açık yani  Open - Closed Principle
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            //bu ilk oluşturluduğu anda bi cacheleme oluşturmaya çalışıyor. True ya da false döner aşağıdaki method eğer true ise out içinde tuttuğu datayı döner.
            //out keywordü birr methodda birden fazla değer dönmeyi sağlar. Tuple da dönebilirdik ama out mantıklı gibi. 
            //aşağıda sadece true falae dönmek isitorum bu method için o yüzden _ yazdım. Memoryde boşuna allocate etmesin.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                //const içinde async method kullanamayız. Sonuna result ekleyip asenkron dönen sonucu senkrona dönüştürmem gerek
                _memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
            //şimdi ise cachelemek gerek. Cache için en uygun data çok sık değişmeyen ama çok sık erişilen datadır.
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new NotFoundExeption($"{typeof(Product).Name} ({id}) not found");
            }
            //task istiyor yani async dönmemi istiyor. Ben bir await işlemi kullanmıyorum o yüzden async de kullanmama gerek yok. Ama benden Task bekliyor o yüzden FromResult methodunu kullandık.
            return Task.FromResult(product);
        }

        public Task<ApiResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productDto = _mapper.Map<List<ProductWithCategoryDTO>>(products);
            return Task.FromResult(ApiResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            //where function istiyor compile expressionu functiona çevirir. Çünkü artık efcore'dan çekmiyorum cacheden bu sprguyu yapıyorum.
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            //bu methodu her çağırdığımda sıfırdan datayı çekip cacheliyor.
            await _memoryCache.Set(CacheProductKey, _repository.GetAll().ToListAsync());
            ;
        }
    }
}
