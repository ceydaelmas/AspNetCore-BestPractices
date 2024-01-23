using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext :DbContext
    {
        //bu options ile beraber veri tabanı yolunu startup dosyasından verebiliyorum.

        //base(options) ifadesi, DbContext sınıfının constructor metodunu çağırır.
        //Bu durumda, DbContext sınıfının kurucu metoduna, options parametresi aracılığıyla gelen yapılandırma seçenekleri ile birlikte çağrı yapılır.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        //default olarak tablonun isminii burdan alır. Categories. configurasyonda değiştirilirse onu alır. 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        //bunu tek ekleyemez product nesnesi üzerinden eklemeli aşağıdaki gibi. var p = new Product(){ProductFeature=new ProductFeature(){}} -best practise. Aslında üstteki satırı kapatmak gerek 
        //deneme amaçlı açıldı.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //burada reflection ile IEntityTypeConfiguration interface'ini implement eden assemblyleri yani classları alıp oradaki konfigürasyonları efcore'a tanımlıyor.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // tek tek bu şekilde de yapabilirdik. modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //model oluşurken çalışan method.
            //diyelim ki category_id yaptık CategoryId yerine bunu burdan ayarlayabilirz. ama ben zaten CategoryId olarak ayarladım

            //modelBuilder.Entity<Category>().HasKey(c => c.Id); //bunlar fluent api. Burdan methoda devam edebiliriz. Id'yi primary olarak ayarladık ama gerek yok aslında.
            //Her entity ile alakalı configürasyonu başka sayfada yapmak mantıklı -CategoryConfiguration
            base.OnModelCreating(modelBuilder);
        }

    }
}
