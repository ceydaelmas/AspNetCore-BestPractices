using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //[ValidateFilterAttribute] bunu buraya eklemek mantıksız. Çünkü bir sürü controllerım varsa tek tek eklemek kötü olur. Globalleştireceğim. Global diyorsak da program.cs
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;//repo katmanını bilmeyecek service katmanından alacak

        public ProductController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }

        //direkt method ismi yazmak yerine  [HttpGet("[action]")] da yazabilirz
        [HttpGet("GetProductsWithCategory")] //GET api/products/GetProductsWithCategory. Buraya ismi ekledik çünkü türe göre anlıyordu yoksa karışırdı. 2 tane Get olduğu zaman hata fırlatır. 
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        //api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDTO= _mapper.Map<List<ProductDTO>>(products.ToList()); //mapping gibi işlemlerin her zaman service katmanına yani business logic tarafında yapılmasıdır. bu düzeltilecek
            return CreateActionResult(ApiResponseDTO<List<ProductDTO>>.Success(200,productsDTO));
        }

        //api/product/5
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productDTO = _mapper.Map<ProductDTO>(product); //mapping gibi işlemlerin her zaman service katmanına yani business logic tarafında yapılmasıdır. bu düzeltilecek
            return CreateActionResult(ApiResponseDTO<ProductDTO>.Success(200, productDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productDTO = _mapper.Map<ProductDTO>(product); //productı alıp geri dtoya dönüştürdük
            return CreateActionResult(ApiResponseDTO<ProductDTO>.Success(201, productDTO)); //201-created
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDTO productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(ApiResponseDTO<NoContentDTO>.Success(204)); //201-created
        }

        //delete api/product/5
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(ApiResponseDTO<NoContentDTO>.Success(204));
        }
    }
}
