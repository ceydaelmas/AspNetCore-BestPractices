using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductServiceWithDto :IServiceWithDto<Product,ProductDTO>
    {
        Task<ApiResponseDTO<List<ProductWithCategoryDTO>>> GetProductsWithCategory();

        //burda bir overload yaptık. artık update methoduna productupdatedto geçebilirm.
        Task<ApiResponseDTO<NoContentDTO>> UpdateAsync(ProductUpdateDTO dto);

        Task<ApiResponseDTO<ProductDTO>> AddAsync(ProductCreateDTO dto);
    }
}
