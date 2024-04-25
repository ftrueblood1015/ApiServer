using Domain.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RepositoryBase<T, TContext> : IRepositoryBase<T>
        where T : EntityBase
        where TContext : DbContext
    {
        protected DbContext Context { get; }

        public RepositoryBase(DbContext context)
        {
            Context = context;
        }

        public virtual T Add(T entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            Context.Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public virtual bool Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            var results = Context.SaveChanges();

            return results > 0;
        }

        public virtual bool DeleteById(Guid entityId)
        {
            var entity = GetById(entityId);

            Context.Set<T>().Remove(entity);
            var results = Context.SaveChanges();

            return results > 0;
        }

        public virtual IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return Context.Set<T>().AsNoTracking().Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual T? GetById(Guid id)
        {
            return Context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual T Update(T entity)
        {
            Context.Set<T>().Update(entity);
            Context.SaveChanges();

            return GetById(entity.Id)!;
        }

        public virtual IEnumerable<T> GetAllExpanded()
        {
            return Context.Set<T>();
        }
    }
}
