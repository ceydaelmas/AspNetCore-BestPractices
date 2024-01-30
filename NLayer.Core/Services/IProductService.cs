using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //özelleştirlmiş bir dto oluşturmam gerek. repolar entity dönerken serviceler dto döner
        Task<ApiResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory();
    }
}
