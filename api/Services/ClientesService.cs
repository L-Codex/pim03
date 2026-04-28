using api.Models;
using api.Repositories;

namespace api.Services
{
    public class ClientesService(ClientesRepository repo)
    {
        private readonly ClientesRepository _repo = repo;

        public async Task<Cliente[]> GetAll()
        {
            var dbClientes = await _repo.GetAll();

            return
            [
                .. dbClientes.Select(c => new Cliente(
                    c.Id,
                    c.Nome,
                    c.Telefone,
                    c.Email,
                    c.CPF,
                    c.DtNascimento.HasValue ? DateOnly.FromDateTime(c.DtNascimento.Value) : null
                )),
            ];
        }
    }
}
