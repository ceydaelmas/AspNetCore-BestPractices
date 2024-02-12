using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IServiceWithDto<Entity,Dto> where Entity : BaseEntity where Dto : class
    {
        Task<ApiResponseDTO<IEnumerable<Dto>>> GetAllAsync();
        Task<ApiResponseDTO<Dto>> GetByIdAsync(int id);
        Task<ApiResponseDTO<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression); //IQuareyble dönersek to list where gibi koşulları controller üzerinden yazabiliyorduk artık yazmamak lazım.
        Task<ApiResponseDTO<bool>> AnyAsync(Expression<Func<Entity, bool>> expression);
        Task<ApiResponseDTO<Dto>> AddAsync(Dto dto);
        Task<ApiResponseDTO<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos);
        //burada veri tabanına değişiklikleri kaydeceğim(savechangeasnyc) yapacağım için void değil task yapıp async yapıyorum
        Task<ApiResponseDTO<NoContentDTO>> UpdateAsync(Dto dto);
        Task<ApiResponseDTO<NoContentDTO>> RemoveAsync(int id);
        Task<ApiResponseDTO<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> ids);
    }
}
