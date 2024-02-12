using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //özelleştirlmiş bir dto oluşturmam gerek. repolar entity dönerken serviceler dto döner
        Task<ApiResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory();

    }
}
