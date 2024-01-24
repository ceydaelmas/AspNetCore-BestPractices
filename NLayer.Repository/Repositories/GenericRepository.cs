using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context; //özel product entitysi için repo oluşturmam da gerekicek o yüzden protected yapıyorum miras aldığım yerlerde bu contexte erişebilmek için..
        private readonly DbSet<T> _dbSet;// readyonlyde sadece ya constructor da ya da burda değer atayabiliyprum başka yerde farklı dbcontext set edilmesin

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            //asnotracing methodu demek oluyor ki efcore çektiği dataları memorye almasın track etmesin daha performanslı çalışsın.
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity); //remove daha db'den silmiyor. sadece memoryde track edip onun stateini deleted olarak flag koyuyoruz. Burda db'den henüz silmiyor. 
            //Save changes yaptığında gidip memoryden flagları bulup silme işlemini yapıyor. Prop state değişince async olmasına hiç gerek yok. Aşağıdakiyle aynı şey:
            //_context.Entry(entity).State = EntityState.Deleted;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
