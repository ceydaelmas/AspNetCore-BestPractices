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
