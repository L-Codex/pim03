using api.Repositories;

namespace api.Services
{
    public class ClientesService(ClientesRepository repo)
    {
        private readonly ClientesRepository _repo = repo;
    }
}
