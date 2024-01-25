using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace NLayer.API.Middlewares
{

    //estension method yazabilmek için class ve method da static olmalı
    public static class UseCustomExceptionHandler
    {
        //IApplicationBuiler için extension method yazıcam tihs ile belirtiyorum.. bunu implement etmiş tum classlarda bunu kullanabilirim 
        public static void UserCustomException(this IApplicationBuilder app)
        {
            //framewrokün bize sağladığı middleware. ben kendi reseponseumu dönmek istiyorum . 
            app.UseExceptionHandler(config =>
            {
                //run isminde ikinci bir middleware yazıyorum .Run sonlandııcı bir middleware(terminal middleware). Bu rundan sonra kış geriye dönecek. Request buraya geldiği anda daha ileriye gitmeyecek.
                //| | | | sırayla bunlara uğrar sonra geri döner response olarak. | = middleware
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();// bunun ile uygulamada fırlatılan hatayı alıyorum tipi ise IExceptionHandlerFeature.
                    //benim kendi throw new Exception ile fırlattığım hata mı yoksa uygulamanın fırlattığı hata mı anlamam gerek. O yüzden bir exception sınıfı yazacağım.
                    //client hatası mı benim uygulamamın mı hatası.
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundExeption => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;
                    var response = ApiResponseDTO<NoContentDTO>.Fail(statusCode, exceptionFeature.Error.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));//burada json dönmem gerek. Controllerda framework otomatik jsona döndürüyordu.
                });
            });
        }
    }
}
