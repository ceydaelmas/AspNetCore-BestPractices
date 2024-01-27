using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        // contructor'da herhangi bir servisi ya da classı DI olarak geçiyorsa bunu program.cs'ye de eklemem gerek. Bu Attribute classını miras almıyor o yüzden validationfilter gibi[Validationfilter] yazamayız. Ve
        //bie filterin ctorunda parametre geçiyorsam direkt kullanamam bunu yerine ServiceFilter üzerinden kullanmam gerek [ServiceFilter(typeof(NotFoundFilter<Product>))]

        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //enspointteki id'yi yakalamam lazım. GetById olarak düşün.

            var idValue = context.ActionArguments.Values.FirstOrDefault(); // id'yi alacak
            if(idValue == null ) {
                await next.Invoke(); //id'si yoksa yoluna devam et. Daha aşağı inmene gerek yok.
                return;
            }
            var id = (int)idValue;
            var anyEntity = await _service.AnyAsync(x => x.Id == id);
            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(ApiResponseDTO<NoContentDTO>.Fail(404, $"{typeof(T).Name}({id}) not found"));
        }
    }
}
