using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        // GET: api/<ClientesController>
        [HttpGet]
        public async void Get() { }

        // GET api/<ClientesController>/5
        [HttpGet("{id}")]
        public async void Get(Guid id) { }

        // POST api/<ClientesController>
        [HttpPost]
        public async void Post([FromBody] string value) { }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public async void Put(Guid id, [FromBody] string value) { }

        [HttpPatch("{id}")]
        public async void Patch(Guid id, [FromBody] string value) { }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{id}")]
        public async void Delete(Guid id) { }
    }
}
