using ApiServer.Attributes;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers.Application
{
    public class ApplicationControllerBase<TEntity, TService> : ControllerBase<TEntity, TService>
        where TEntity : EntityBase
        where TService : IServiceBase<TEntity>
    {
        public ApplicationControllerBase(TService service) : base(service)
        {
        }

        [HttpPost]
        [RequiredRole("SuperUser")]
        public override ActionResult<TEntity> Add([FromBody] TEntity entity)
        {
            return base.Add(entity);
        }

        [HttpDelete("{id}")]
        [RequiredRole("SuperUser")]
        public override ActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [RequiredRole("SuperUser")]
        public override ActionResult<IEnumerable<TEntity>> GetAll()
        {
            return base.GetAll();
        }

        [HttpGet("expand")]
        [RequiredRole("SuperUser")]
        public override ActionResult<IEnumerable<TEntity>> GetAllExpanded()
        {
            return base.GetAllExpanded();
        }

        [HttpGet("filter")]
        [RequiredRole("SuperUser")]
        public override ActionResult<IEnumerable<TEntity>> Filter(Func<TEntity, bool> predicate)
        {
            return base.Filter(predicate);
        }

        [HttpGet("{id}")]
        [RequiredRole("SuperUser")]
        public override ActionResult<TEntity> GetEntity(Guid id)
        {
            return base.GetEntity(id);
        }

        [HttpPut]
        [RequiredRole("SuperUser")]
        public override ActionResult<TEntity> Update([FromBody] TEntity entity)
        {
            return base.Update(entity);
        }
    }
}
