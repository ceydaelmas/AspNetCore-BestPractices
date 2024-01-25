using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class CategoryWithProductsDTO : CategoryDTO
    {
        public List<ProductDTO> Products { get; set; }
    }
}
