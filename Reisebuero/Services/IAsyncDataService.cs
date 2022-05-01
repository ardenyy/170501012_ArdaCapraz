using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reisebuero.Services
{
    public interface IAsyncDataService<T>
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> UpdateAsync(int id, T entity);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
