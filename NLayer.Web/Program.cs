using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using NLayer.Web.Filters;
using NLayer.Web.Modules;
using NLayer.Web.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        //direkt assembly ad�n� yazmak yerine oldu�u yeri bu �ekil tan�mlad�k daha dinamik. AppDbcontextin oldu�u assmeblyi al�yor.
        //NLayer.Repository
    });
    //App dbcontextimin oldu�u assemblyi tan�tmam laz�m.repository katman�nda
});
builder.Services.AddHttpClient<ProductApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
//BEST PRACTISE
//http clienti kendin �retme bu k�t� y�ntem DI container'a b�rak bu i�i new ile �retme. DI container bize nesne �rne�i veryor. Daha performansl� ve daha az nesne �rne�i �reterek Http clienti kullanabilir zb�ylece soket yoku�u hatalar�ndan y�rtar�z :)
builder.Services.AddHttpClient<CategoryApiService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddScoped(typeof(NotFoundFilter<>));//e�er b�yle bir T �eklinde tip al�yorsa yani genericse bunu programcsde belirtmemiz gerekiyor.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();
app.UseExceptionHandler("/Home/Error");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error"); bunu d��ar� alal�m ��nk� bizim ortam�m�z �u anda development error sayfas�n� g�relim
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
