using System.Linq;

namespace WhoScored.Model.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Save(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
