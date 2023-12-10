using System.Linq.Expressions;

namespace DataLayer.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllByFilter(Expression<Func<T, bool>>? filter=null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null);
        void Add(T entity);
        void Remove(T entity);
    }
}
