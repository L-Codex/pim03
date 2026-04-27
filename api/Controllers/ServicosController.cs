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
        public async Task<ActionResult<Servico>> Post([FromBody] ServicoCreateDTO value)
        {
            return await _service.CreateOne(value);
        }

        // PUT api/<ServicosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

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
