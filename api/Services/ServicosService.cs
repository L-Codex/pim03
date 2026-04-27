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

        public async Task<Servico[]> GetAll()
        {
            var dbServicos = await _repo.GetAll();

            return dbServicos
                .Select(s => new Servico(s.Id, s.Nome, s.Descricao, s.Preco))
                .ToArray();
        }

        public async Task<Servico?> GetOne(Guid id)
        {
            var result = await _repo.GetOne(id);

            if (!result.HasValue)
            {
                return null;
            }

            var service = result.Value;

            return new Servico(service.Id, service.Nome, service.Descricao, service.Preco);
        }
    }
}
