using api.DTOs;
using api.Models;
using api.Repositories;
using api.Utilities;

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

        public async Task<Cliente?> GetOne(Guid id)
        {
            var result = await _repo.GetOne(id);

            if (!result.HasValue)
            {
                return null;
            }

            var c = result.Value;

            return new Cliente(
                c.Id,
                c.Nome,
                c.Telefone,
                c.Email,
                c.CPF,
                c.DtNascimento.HasValue ? DateOnly.FromDateTime(c.DtNascimento.Value) : null
            );
        }

        public async Task<Maybe<bool>> CreateOne(ClienteCreateDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.CPF))
            {
                var cpfExists = await _repo.CheckCPF(dto.CPF);

                if (cpfExists)
                {
                    return new Maybe<bool>(ErrorCodes.AlreadyExists);
                }
            }

            var newCliente = new Cliente(
                null,
                dto.Nome,
                dto.Telefone,
                dto.CPF,
                dto.Email,
                dto.DtNascimento
            );

            var created = await _repo.CreateOne(
                new ClienteDTO(
                    newCliente.Id,
                    newCliente.Nome,
                    newCliente.Telefone,
                    newCliente.CPF,
                    newCliente.Email,
                    newCliente.DtNascimento.HasValue
                        ? newCliente.DtNascimento.Value.ToDateTime(new TimeOnly(0, 0))
                        : null
                )
            );

            if (created)
            {
                return new Maybe<bool>(true);
            }

            // TODO: Lidar com erros
            throw new Exception("Erro ao criar cliente.");
        }
    }
}
