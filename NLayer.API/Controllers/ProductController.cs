using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;//repo katmanını bilmeyecek service katmanından alacak

        public ProductController(IMapper mapper, IService<Product> service)
        {
            _mapper = mapper;
            _service = service;
        }
        //api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = _service.GetAllAsync();
            var productsDTO= _mapper.Map<List<ProductDTO>>(products); //mapping gibi işlemlerin her zaman service katmanına yani business logic tarafında yapılmasıdır. bu düzeltilecek
            return CreateActionResult(ApiResponseDTO<List<ProductDTO>>.Success(200,productsDTO));
        }

        //api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = _service.GetByIdAsync(id);
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
            return CreateActionResult(ApiResponseDTO<NoContentDTO>.Success(201)); //201-created
        }

        //delete api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(ApiResponseDTO<NoContentDTO>.Success(204));
        }
    }
}
