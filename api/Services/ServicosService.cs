using api.Models;
using api.Repositories;
using api.Utilities;

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

        public async Task<Maybe<bool>> DeleteOne(Guid id)
        {
            return await _repo.DeleteOne(id);
        }

        public async Task<Servico> CreateOne(ServicoCreateDTO dto)
        {
            var newServico = new Servico(null, dto.Nome, dto.Descricao, dto.Preco);

            var created = await _repo.CreateOne(
                new ServicoDTO(
                    newServico.Id,
                    newServico.Nome,
                    newServico.Descricao,
                    newServico.Preco
                )
            );

            if (created)
            {
                return newServico;
            }

            // TODO: Lidar com erros
            throw new Exception("Erro ao criar serviço.");
        }
    }
}
