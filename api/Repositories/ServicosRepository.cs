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

        public async Task<Servico[]> ListarTodos()
        {
            var servicos = new List<Servico>();

            await using var connection = await _ds.OpenConnectionAsync();

            await using var command = new NpgsqlCommand(
                "SELECT nome, descricao, valor FROM tb_servico",
                connection
            );

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                servicos.Add(
                    new Servico(
                        reader.GetString(0),
                        reader.IsDBNull(1) ? null : reader.GetString(1),
                        reader.GetDouble(2)
                    )
                );
            }

            return servicos.ToArray();
        }
    }
}
