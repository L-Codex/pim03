using api.Models;
using api.Utilities;
using Npgsql;

namespace api.Repositories
{
    public class ServicosRepository
    {
        private readonly NpgsqlDataSource _ds;

        public ServicosRepository(NpgsqlDataSource ds)
        {
            _ds = ds;
        }

        public async Task<ServicoDTO[]> GetAll()
        {
            var servicos = new List<ServicoDTO>();

            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(
                "SELECT id, nome, descricao, valor FROM tb_servico",
                connection
            );

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                servicos.Add(
                    new ServicoDTO(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.IsDBNull(2) ? null : reader.GetString(2),
                        reader.GetDouble(3)
                    )
                );
            }

            return servicos.ToArray();
        }

        public async Task<Maybe<ServicoDTO>> GetOne(Guid id)
        {
            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(
                "SELECT id, nome, descricao, valor FROM tb_servico WHERE id = $1",
                connection
            );
            command.Parameters.AddWithValue(id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Maybe<ServicoDTO>(
                    new ServicoDTO(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.IsDBNull(2) ? null : reader.GetString(2),
                        reader.GetDouble(3)
                    )
                );
            }

            return new Maybe<ServicoDTO>(ErrorCodes.NotFound);
        }

        public async Task<Maybe<bool>> DeleteOne(Guid id)
        {
            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(
                "DELETE FROM tb_servico WHERE id = $1",
                connection
            );
            command.Parameters.AddWithValue(id);

            int affectedRows;

            try
            {
                affectedRows = await command.ExecuteNonQueryAsync();
            }
            catch (PostgresException ex) when (ex.SqlState == "23001")
            {
                // restrict_violation: O serviço está sendo referenciado por um agendamento.
                return new Maybe<bool>(ErrorCodes.CantModify);
            }
            if (affectedRows == 0)
            {
                return new Maybe<bool>(ErrorCodes.NotFound);
            }

            return new Maybe<bool>(true);
        }

        public async Task<bool> CreateOne(ServicoDTO dto)
        {
            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(
                "INSERT INTO tb_servico (id, nome, descricao, valor) VALUES ($1, $2, $3, $4) RETURNING id",
                connection
            );
            command.Parameters.AddWithValue(dto.Id);
            command.Parameters.AddWithValue(dto.Nome);
            command.Parameters.AddWithValue((object?)dto.Descricao ?? DBNull.Value);
            command.Parameters.AddWithValue(dto.Preco);

            await command.ExecuteNonQueryAsync();

            return true;
        }
    }
}
