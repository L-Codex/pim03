using api.Models;
using api.Repositories;

namespace api.Services
{
    public class ServicosService
    {
        private readonly ServicosRepository _repo;

        public ServicosService(ServicosRepository repo)
        {
            _repo = repo;
        }

        public async Task<Servico[]> Listar()
        {
            return await _repo.ListarTodos();
        }
    }
}
