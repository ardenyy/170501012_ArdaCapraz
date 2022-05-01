using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reisebuero.Models;

namespace Reisebuero.Services
{
    public class AsyncGenericDataService<T> : IAsyncDataService<T> where T : BaseModel
    {
        private readonly ReisebueroDbContextFactory _contextFactory;

        public AsyncGenericDataService(ReisebueroDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> CreateAsync(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            var createdEntity = await context.Set<T>().AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return createdEntity.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            T entity = await context.Set<T>().SingleAsync(e => e.ID.Equals(id)).ConfigureAwait(false);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<T?> GetAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            T? entity = await context.Set<T>().SingleOrDefaultAsync(e => e.ID.Equals(id)).ConfigureAwait(false);
            await Task.Delay(2000).ConfigureAwait(false);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            IEnumerable<T> entities = await context.Set<T>().ToListAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            entity.ID = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }
    }
}
