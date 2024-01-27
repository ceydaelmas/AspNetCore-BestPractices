using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
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

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());//nerde oldu�unun parametresini veriyoruz. Class� veriyoruz o class�n oldu�u assemblyi al�yor. Yani bu assemblydeki t�m bu Abstrac Validationu inherit etmi� classlar� getirir.

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;//kendi d�nd��� modeli pasif hale getirdim. FluentValidationun filtresini pasife �ektim.
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();//caching i�in ekledik.

builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));//mapping dosyam nerdeyse onu vermem gerek yani Map Profile. MapProfile isminden assemblyi bulcak.

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //IUnitofwork ile kar��la��rsa unitofwork� esas al�cak.
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //Generic tipte oldu�u i�in typeof yap�yoruz. generic t�r� var. Bu durumda, hangi generic t�rle �al���laca�� bilinmiyor ��nk� bu bir generic t�r.
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();//bunlar DI containera eklemi� olyoruz

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        //direkt assembly ad�n� yazmak yerine oldu�u yeri bu �ekil tan�mlad�k daha dinamik. AppDbcontextin oldu�u assmeblyi al�yor.
        //NLayer.Repository
    });
    //App dbcontextimin oldu�u assemblyi tan�tmam laz�m.repository katman�nda
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//bunlar middleware. Bu middlewarelerden ge�e ge�e controllerdaki action methoda gelir. Actiondan sonu� �retildikten sonra middlewarelere tekrar u�rayarak response olu�ur.
app.UseHttpsRedirection();//http ile ba�layan bir url varsa bunu https'e y�nlendirir.

app.UserCustomException();

app.UseAuthorization();//token do�rulamas� burda ger�ekle�tirir bir istek geldi�inde

app.MapControllers();

app.Run();
