namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        //int,decimal vs default değerleri var 0 o yüzden null olabilir diye uyarı yok. ama string category vs referans tipler olduğu için null olabilir.
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        //Foreign Key (EFCore CategoryId yapınca otomatik algılıyor ve foreign key olarak belirliyor. Aynı şekilde BaseEntitydeki Id'yi de Primary olarak algılıyor.
        //Ve aşağıdaki gibi yapmamıza gerek kalmıyor- Best Practise)
        //public int _CategoryId { get; set; }
        //[ForeignKey("_CategoryId")]
        public Category Category { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
