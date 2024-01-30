namespace NLayer.Core.DTOs
{
    public class CategoryWithProductsDTO : CategoryDTO
    {
        public List<ProductDTO> Products { get; set; }
    }
}
