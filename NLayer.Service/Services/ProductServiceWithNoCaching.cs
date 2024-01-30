﻿using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithNoCaching : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductServiceWithNoCaching(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ApiResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory()
        {
            var products = await _productRepository.GetProductsWithCategory();
            var productsDTO = _mapper.Map<List<ProductWithCategoryDTO>>(products);
            //business burda döneceği için direkt api responselarını burda dönelim. API katmanıda daha az iş olsun.
            return ApiResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsDTO);
        }
    }
}
