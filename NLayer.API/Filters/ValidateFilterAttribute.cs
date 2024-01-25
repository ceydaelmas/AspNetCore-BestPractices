using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //context.modelState.isvalid bu demek ki her şey yolunda ture ise. Yani validasyon yapmasan bile burdan kontrol edebilirsin. Direkt olarak modelState içinde hatalar yükleniyor.
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x=>x.ErrorMessage).ToList(); //dictionary dönüyor ama ben hataları alıyorum.
                //Select many size bir dictionary geliyorsa ordan tek bir propertyi almamıza imkan veriyor. Select ise bir sınıftan hata mesjaını alıyor.
                context.Result = new BadRequestObjectResult(ApiResponseDTO<NoContentDTO>.Fail(400,errors));
            }
        }
    }
}
