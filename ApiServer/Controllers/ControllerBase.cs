using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControllerBase<TEntity, TService> : ControllerBase
        where TEntity : EntityBase
        where TService : IServiceBase<TEntity>
    {
        protected TService Service { get; private set; }

        public ControllerBase(TService service)
        {
            Service = service;
        }

        [HttpPost]
        public virtual ActionResult<TEntity> Add([FromBody] TEntity entity)
        {
            var response = Service.Add(entity);
            return CreatedAtAction(nameof(Add), response);
        }

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(Guid id)
        {
            var result = Service.DeleteById(id);
            return result ? Ok() : BadRequest();
        }

        [HttpGet]
        public virtual ActionResult<IEnumerable<TEntity>> GetAll()
        {
            var result = Service.GetAll();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("expand")]
        public virtual ActionResult<IEnumerable<TEntity>> GetAllExpanded()
        {
            var result = Service.GetAllExpanded();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("filter")]
        public virtual ActionResult<IEnumerable<TEntity>> Filter(Func<TEntity, bool> predicate)
        {
            var result = Service.Filter(predicate);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual ActionResult<TEntity> GetEntity(Guid id)
        {
            var result = Service.GetById(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return Ok(result);
        }

        [HttpPut]
        public virtual ActionResult<TEntity> Update([FromBody] TEntity entity)
        {
            var result = Service.Update(entity);
            return CreatedAtAction(nameof(Update), result);
        }
    }
}
