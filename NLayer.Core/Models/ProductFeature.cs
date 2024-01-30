namespace NLayer.Core.Models
{

    //product'a bağlı olduğu için BaseEntity vermeme gerek yok. Oluşturulma tarihi zaten productin olduşturulduğu tarihtir.
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
