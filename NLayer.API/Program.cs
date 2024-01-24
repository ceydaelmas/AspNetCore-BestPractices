using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWork;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //IUnitofwork ile kar��la��rsa unitofwork� esas al�cak.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //Generic tipte oldu�u i�in typeof yap�yoruz. generic t�r� var. Bu durumda, hangi generic t�rle �al���laca�� bilinmiyor ��nk� bu bir generic t�r.
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();//bunlar DI containera eklemi� olyoruz


builder.Services.AddAutoMapper(typeof(MapProfile));//mapping dosyam nerdeyse onu vermem gerek yani Map Profile. MapProfile isminden assemblyi bulcak.


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
