using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public void Post([FromBody] string value) { }

        // PUT api/<ServicosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/<ServicosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
