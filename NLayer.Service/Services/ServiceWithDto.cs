using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ServiceWithDto<Entity, Dto> : IServiceWithDto<Entity, Dto> where Entity : BaseEntity where Dto : class
    {
        private readonly IGenericRepository<Entity> _repository;
        protected readonly IUnitOfWork _unitOfWork; // mesela category service bundan miras alıcan bunu protected yaparsak üst sınıflarda da kullanabilelim
        protected readonly IMapper _mapper;

        public ServiceWithDto(IGenericRepository<Entity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponseDTO<Dto>> AddAsync(Dto dto)
        {
            Entity newEntity = _mapper.Map<Entity>(dto);
            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = _mapper.Map<Dto>(newEntity);
            return ApiResponseDTO<Dto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<ApiResponseDTO<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos)
        {
            var newEntities = _mapper.Map<IEnumerable<Entity>>(dtos);
            await _repository.AddRangeAsync(newEntities);
            await _unitOfWork.CommitAsync();
            var newDtos = _mapper.Map<IEnumerable<Dto>>(newEntities);
            return ApiResponseDTO<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, newDtos);
        }

        public async Task<ApiResponseDTO<bool>> AnyAsync(Expression<Func<Entity, bool>> expression)
        {
            var anyEntity = await _repository.AnyAsync(expression);
            return ApiResponseDTO<bool>.Success(StatusCodes.Status200OK, anyEntity);
        }

        public async Task<ApiResponseDTO<IEnumerable<Dto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync() ;
            var dtos = _mapper.Map<IEnumerable<Dto>>(entities);
            return ApiResponseDTO<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<ApiResponseDTO<Dto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            var dto = _mapper.Map<Dto>(entity);
            return ApiResponseDTO<Dto>.Success(StatusCodes.Status200OK,dto); 
        }

        public async Task<ApiResponseDTO<NoContentDTO>> RemoveAsync(int id)
        {
            var entity = await  _repository.GetByIdAsync(id);
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return ApiResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<ApiResponseDTO<NoContentDTO>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var entities = await _repository.Where(x => ids.Contains(x.Id)).ToListAsync();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            return ApiResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<ApiResponseDTO<NoContentDTO>> UpdateAsync(Dto dto)
        {
            var entity = _mapper.Map<Entity>(dto);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return ApiResponseDTO<NoContentDTO>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<ApiResponseDTO<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression)
        {
            var entities = await _repository.Where(expression).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<Dto>>(entities);
            return ApiResponseDTO<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }
    }
}
