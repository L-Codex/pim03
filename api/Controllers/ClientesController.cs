using api.Models;
using api.Services;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController(ClientesService service) : ControllerBase
    {
        private readonly ClientesService _service = service;

        // GET: /api/clientes
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cliente>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<Cliente[]>>> Get()
        {
            var clientes = await _service.GetAll();
            return Ok(ApiResponse.Ok(clientes));
        }

        // GET: /api/clientes/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Cliente>>> Get(Guid id)
        {
            var cliente = await _service.GetOne(id);

            if (cliente is null)
                return NotFound(
                    ApiResponse.Fail<Cliente>(new ApiError("NOT_FOUND", "Cliente não encontrado."))
                );

            return Ok(ApiResponse.Ok(cliente));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] ClienteCreateDTO dto)
        {
            var created = await _service.CreateOne(dto);

            if (!created)
            {
                return BadRequest(
                    ApiResponse.Fail<object>(
                        new ApiError("CREATE_FAILED", "Erro ao criar o cliente.")
                    )
                );
            }

            return NoContent();
        }

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
