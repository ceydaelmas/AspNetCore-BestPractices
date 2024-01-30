using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction] //bu bir endpoint değil o yüzden no action koyuyorum.Swaggerda çıkmayacak
        public IActionResult CreateActionResult<T>(ApiResponseDTO<T> apiResponse)
        {
            if (apiResponse.StatusCode == 204)//no content 
                return new ObjectResult(null)
                {
                    StatusCode = apiResponse.StatusCode
                };
            return new ObjectResult(apiResponse)
            {
                StatusCode = apiResponse.StatusCode
            };
        }
    }
}
