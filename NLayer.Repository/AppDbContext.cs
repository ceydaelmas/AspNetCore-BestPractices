﻿using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        //bu options ile beraber veri tabanı yolunu startup dosyasından verebiliyorum.

        //base(options) ifadesi, DbContext sınıfının constructor metodunu çağırır.
        //Bu durumda, DbContext sınıfının kurucu metoduna, options parametresi aracılığıyla gelen yapılandırma seçenekleri ile birlikte çağrı yapılır.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

            //burdan da seed data ekleyebilirz.ama best practise değil.
            modelBuilder.Entity<ProductFeature>().HasData(
              new ProductFeature
              {
                  Id = 1,
                  Color = "kırmızı",
                  Height = 100,
                  Width = 50,
                  ProductId = 1
              });


            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            //veri tabanına yansıtmadan hemen önceki işlemleri burda yapabilirim. Update veya created date işlemi 
            //ilk track edilen entitiyleri tek tek dönelim 
            foreach(var item in ChangeTracker.Entries())
            {
                //eğer bu item bir base entity ise onu alıp tipini anladık.
                if(item.Entity is BaseEntity entityReferennce)
                {
                    //şimdi ise güncelleme veya update var mı onu ölçeceğiz
                    switch (item.State) {
                        case EntityState.Added:
                            {
                                entityReferennce.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReferennce).Property(x => x.CreatedDate).IsModified = false;//güncelleme yaparken bu alana dokunma bu şekilde kalsın.
                                entityReferennce.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                //eğer bu item bir base entity ise onu alıp tipini anladık.
                if (item.Entity is BaseEntity entityReferennce)
                {
                    //şimdi ise güncelleme veya update var mı onu ölçeceğiz
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReferennce.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entityReferennce.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }

    }
}
