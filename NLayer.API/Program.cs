using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());//nerde olduðunun parametresini veriyoruz. Classý veriyoruz o classýn olduðu assemblyi alýyor. Yani bu assemblydeki tüm bu Abstrac Validationu inherit etmiþ classlarý getirir.

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;//kendi döndüðü modeli pasif hale getirdim. FluentValidationun filtresini pasife çektim.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //IUnitofwork ile karþýlaþýrsa unitofworkü esas alýcak.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //Generic tipte olduðu için typeof yapýyoruz. generic türü var. Bu durumda, hangi generic türle çalýþýlacaðý bilinmiyor çünkü bu bir generic tür.
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();//bunlar DI containera eklemiþ olyoruz

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAutoMapper(typeof(MapProfile));//mapping dosyam nerdeyse onu vermem gerek yani Map Profile. MapProfile isminden assemblyi bulcak.


builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        //direkt assembly adýný yazmak yerine olduðu yeri bu þekil tanýmladýk daha dinamik. AppDbcontextin olduðu assmeblyi alýyor.
        //NLayer.Repository
    });
    //App dbcontextimin olduðu assemblyi tanýtmam lazým.repository katmanýnda
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
