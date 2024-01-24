using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
               new Product { Id = 1, CategoryId = 1, Price = 100, Stock = 20, CreatedDate = DateTime.Now, Name = "kalem 1" },
               new Product { Id = 2, CategoryId = 1, Price = 200, Stock = 10, CreatedDate = DateTime.Now, Name = "kalem 2" },
               new Product { Id = 3, CategoryId = 2, Price = 500, Stock = 40, CreatedDate = DateTime.Now, Name = "Kitap 1" });
        }
    }
}
