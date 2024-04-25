using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Services
{
    public class ServiceBase<T, TRepo> : IServiceBase<T>
        where T : EntityBase
        where TRepo : IRepositoryBase<T>
    {
        protected IRepositoryBase<T> Repo { get; }

        public ServiceBase(IRepositoryBase<T> repo)
        {
            Repo = repo;
        }

        public virtual T Add(T entity)
        {
            try
            {
                return Repo.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
                return Repo.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual bool DeleteById(Guid entityId)
        {
            try
            {
                return Repo.DeleteById(entityId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            try
            {
                return Repo.Filter(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return Repo.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual T? GetById(Guid id)
        {
            try
            {
                return Repo.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                return Repo.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<T> GetAllExpanded()
        {
            try
            {
                return Repo.GetAllExpanded();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
