using api.Models;
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

        public async Task<ServicoDTO[]> ListarTodos()
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
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.IsDBNull(2) ? null : reader.GetString(2),
                        reader.GetDouble(3)
                    )
                );
            }

            return servicos.ToArray();
        }
    }
}
