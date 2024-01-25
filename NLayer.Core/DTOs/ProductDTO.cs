using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class ProductDTO : BaseDTO
    {
        //[Required(ErrorMessage ="alan gerekli")]
        //[Range(1,100)]
        //bu şekilde validasyon uygun değil çünkü  yönetmek zorlaşacak.  Busness işlemi olduğu için servis katmanında yapacağım.
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
