namespace NLayer.Core.DTOs
{
    //bu best practice değil her bir işlem için dto oluşturmak.
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
