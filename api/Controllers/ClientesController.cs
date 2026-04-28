using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        // GET: /api/clientes
        [HttpGet]
        public async void Get() { }

        // GET api/clientes/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpGet("{id}")]
        public async void Get(Guid id) { }

        // POST /api/clientes
        [HttpPost]
        public async void Post([FromBody] string value) { }

        // PUT /api/clientes/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpPut("{id}")]
        public async void Put(Guid id, [FromBody] string value) { }

        // PATCH /api/clientes/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpPatch("{id}")]
        public async void Patch(Guid id, [FromBody] string value) { }

        // DELETE /api/clientes/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpDelete("{id}")]
        public async void Delete(Guid id) { }
    }
}
