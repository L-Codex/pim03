using api.Models;
using api.Repositories;
using api.Utilities;

namespace api.Services
{
    public class ServicosService(ServicosRepository repo)
    {
        private readonly ServicosRepository _repo = repo;

        public async Task<Servico[]> GetAll()
        {
            var dbServicos = await _repo.GetAll();

            return [.. dbServicos.Select(s => new Servico(s.Id, s.Nome, s.Descricao, s.Preco))];
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

        public async Task<bool> CreateOne(ServicoCreateDTO dto)
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
                return true;
            }

            // TODO: Lidar com erros
            throw new Exception("Erro ao criar serviço.");
        }

        public async Task<Maybe<bool>> UpdateOne(Guid id, ServicoUpdateDTO dto)
        {
            var existingResult = await _repo.GetOne(id);

            if (!existingResult.HasValue)
            {
                return new Maybe<bool>(ErrorCodes.NotFound);
            }

            var existing = existingResult.Value;

            var updatedServico = new ServicoDTO(
                id,
                dto.Nome ?? existing.Nome,
                dto.Descricao ?? existing.Descricao,
                dto.Preco ?? existing.Preco
            );

            var updateResult = await _repo.UpdateOne(updatedServico);

            if (!updateResult)
            {
                return new Maybe<bool>(ErrorCodes.NotFound);
            }

            return new Maybe<bool>(true);
        }

        public async Task<bool> ReplaceOne(Guid id, ServicoCreateDTO dto)
        {
            var updated = await _repo.UpdateOne(
                new ServicoDTO(id, dto.Nome, dto.Descricao, dto.Preco)
            );

            if (updated)
            {
                return true;
            }

            // TODO: Lidar com erros.
            throw new Exception("Erro ao atualizar serviço.");
        }
    }
}
