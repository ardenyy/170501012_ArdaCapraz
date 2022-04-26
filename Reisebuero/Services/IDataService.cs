using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reisebuero.Services
{
    public interface IDataService<T>
    {
        Task<T> Create(T entity);
        Task Delete(int id);
        Task<T> Update(int id, T entity);
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
    }
}
