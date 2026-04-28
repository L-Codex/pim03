using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly ServicosService _service;

        public ServicosController(ServicosService service)
        {
            _service = service;
        }

        // GET: api/<ServicosController>
        [HttpGet]
        public async Task<IEnumerable<Servico>> Get()
        {
            return await _service.GetAll();
        }

        // GET api/<ServicosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> Get(Guid id)
        {
            var serv = await _service.GetOne(id);
            if (serv == null)
            {
                return NotFound();
            }
            return Ok(serv);
        }

        // POST api/<ServicosController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ServicoCreateDTO value)
        {
            var created = await _service.CreateOne(value);
            if (created)
            {
                return NoContent();
            }
            return BadRequest("Erro ao criar o serviço.");
        }

        // PUT api/<ServicosController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] ServicoCreateDTO value)
        {
            var updated = await _service.ReplaceOne(id, value);
            if (updated)
            {
                return NoContent();
            }
            return BadRequest("Erro ao atualizar o serviço.");
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] ServicoUpdateDTO value)
        {
            var updated = await _service.UpdateOne(id, value);
            if (updated.HasValue)
            {
                return NoContent();
            }
            if (updated.HasError)
            {
                switch (updated.Error)
                {
                    case Utilities.ErrorCodes.NotFound:
                        return NotFound();
                    default:
                        throw new Exception("Erro desconhecido.");
                }
            }
            return BadRequest("Erro ao atualizar o serviço.");
        }

        // DELETE api/<ServicosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteOne(id);

            if (result.HasError)
            {
                switch (result.Error)
                {
                    case Utilities.ErrorCodes.NotFound:
                        return NotFound();
                    case Utilities.ErrorCodes.CantModify:
                        return Conflict("Não é possível modificar este recurso.");
                    default:
                        return StatusCode(500, "Erro desconhecido.");
                }
            }

            return Ok();
        }
    }
}
