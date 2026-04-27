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
            var dbServicos = await _repo.ListarTodos();

            return dbServicos
                .Select(s => new Servico(s.Id, s.Nome, s.Descricao, s.Preco))
                .ToArray();
        }
    }
}
