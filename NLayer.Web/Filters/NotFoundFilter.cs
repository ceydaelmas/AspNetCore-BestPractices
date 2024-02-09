using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //enspointteki id'yi yakalamam lazım. GetById olarak düşün.
            var idValue = context.ActionArguments.Values.FirstOrDefault(); // id'yi alacak
            if (idValue == null)
            {
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

            var errorViewModel = new MVCErrorViewModel();
            errorViewModel.Errors.Add($"{typeof(T).Name}({id}) not found");

            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
