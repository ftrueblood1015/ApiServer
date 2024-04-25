using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        T Add(T entity);

        bool Delete(T entity);

        bool DeleteById(Guid entityId);

        IEnumerable<T> Filter(Func<T, bool> predicate);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllExpanded();

        T? GetById(Guid id);

        T Update(T entity);
    }
}
