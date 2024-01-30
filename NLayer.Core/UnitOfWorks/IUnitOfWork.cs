namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //veri tabanına yapılacak işlemleri toplu bir şekilde tek bir transaction üzerinden yönetmemize izin verir
        Task CommitAsync();
        void Commit();
    }
}
