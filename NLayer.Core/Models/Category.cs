namespace NLayer.Core.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } // Farklı entity ya da classa referans verdiğimiz propertyler navigation property olarak geçer.

    }
}
