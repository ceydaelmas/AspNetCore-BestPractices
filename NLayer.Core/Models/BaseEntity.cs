namespace NLayer.Core.Models
{
    //abstract nesne oluşturulmaması için yaptım.newlenemez(interface için de aynı
    public abstract class BaseEntity
    {
        // Id yapmassam bu attribute eklemem gerekecekti : [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
