using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {

        //Ienumareble dönmememin debebi üzerinde sonrasında işlem yapıp tolist yapmak.
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int id);

        //Iqueryable döndüğümüzde yazmış olduğum sorgular direkt veri tabanına gitmez. To list ya da Tolistasync gibi methodları çağırısam o zaman db'ye gider.
        //Başka sorgular da yazmak için böyle yapıyorum(mesela where ya da order By) daha performanslı oluyor. Bu işlemlerden sonra ToList diyebilirim.
        //productRepository.Where(x=>x.id>5) burdan ıquerybale dönüyor.  .ToListAsync() dersem db'ye gider.

        //sorgu-> expression olcak ve function Delegate alcak. T şekilde nesne alcak ve boolean döncek. Delegate methodları işaret eden yapılardır(action da bir Delegate)
        //Delege, metodların adreslerini yani metodları fiziksel olarak işaretleyebilen yapılar olduğunu hatırlayalım. Func<int, int> Toplam = ToplamMetodu;
        IQueryable<T> Where(Expression<Func<T,bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities); //birden fazla eklemek için

        //update veya remove'un metodu yok efcore'da olmasına da gerek yok. Classın sadece stateini değiştiriyor uzun süren bi işlem olmadığı için async olmasına gerek yok.
        //async'yi neden kullanıyoruz var olan threadleri blocklamamak ve daha efektif kullanmak için async programlama yapılıyor
        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

    }
}
