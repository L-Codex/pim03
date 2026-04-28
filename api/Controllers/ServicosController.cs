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

        // GET: /api/servicos
        [HttpGet]
        public async Task<IEnumerable<Servico>> Get()
        {
            return await _service.GetAll();
        }

        // GET /api/servicos/3fa85f64-5717-4562-b3fc-2c963f66afa6
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

        // POST /api/servicos
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

        // PUT /api/servicos/3fa85f64-5717-4562-b3fc-2c963f66afa6
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

        // PATCH /api/servicos/3fa85f64-5717-4562-b3fc-2c963f66afa6
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
                return updated.Error switch
                {
                    Utilities.ErrorCodes.NotFound => NotFound(),
                    _ => throw new Exception("Erro desconhecido."),
                };
            }
            return BadRequest("Erro ao atualizar o serviço.");
        }

        // DELETE /api/servicos/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteOne(id);

            if (result.HasError)
            {
                return result.Error switch
                {
                    Utilities.ErrorCodes.NotFound => NotFound(),
                    Utilities.ErrorCodes.CantModify => Conflict(
                        "Não é possível modificar este recurso."
                    ),
                    _ => StatusCode(500, "Erro desconhecido."),
                };
            }

            return Ok();
        }
    }
}
