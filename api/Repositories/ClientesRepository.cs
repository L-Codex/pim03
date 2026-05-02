using api.Models;
using api.Utilities;
using Npgsql;

namespace api.Repositories
{
    public class ClientesRepository(NpgsqlDataSource ds)
    {
        private readonly NpgsqlDataSource _ds = ds;

        public async Task<ClienteDTO[]> GetAll()
        {
            var clientes = new List<ClienteDTO>();

            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(Queries.GET_ALL_CLIENTES, connection);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientes.Add(
                    new ClienteDTO(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.IsDBNull(3) ? null : reader.GetString(3),
                        reader.IsDBNull(4) ? null : reader.GetString(4),
                        reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                    )
                );
            }

            return [.. clientes];
        }

        public async Task<Maybe<ClienteDTO>> GetOne(Guid id)
        {
            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(Queries.GET_CLIENTE_BY_ID, connection);
            command.Parameters.AddWithValue(id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Maybe<ClienteDTO>(
                    new ClienteDTO(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.IsDBNull(3) ? null : reader.GetString(3),
                        reader.IsDBNull(4) ? null : reader.GetString(4),
                        reader.IsDBNull(5) ? null : reader.GetDateTime(5)
                    )
                );
            }

            return new Maybe<ClienteDTO>(ErrorCodes.NotFound);
        }

        public async Task<bool> CreateOne(ClienteDTO dto)
        {
            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(Queries.INSERT_CLIENTE, connection);

            command.Parameters.AddWithValue(dto.Id);
            command.Parameters.AddWithValue(dto.Nome);
            command.Parameters.AddWithValue((object?)dto.Telefone ?? DBNull.Value);
            command.Parameters.AddWithValue((object?)dto.Email ?? DBNull.Value);
            command.Parameters.AddWithValue((object?)dto.CPF ?? DBNull.Value);
            command.Parameters.AddWithValue((object?)dto.DtNascimento ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();

            return true;
        }
    }
}
