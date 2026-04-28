using Npgsql;

namespace api.Repositories
{
    public class ClientesRepository(NpgsqlDataSource ds)
    {
        private readonly NpgsqlDataSource _ds = ds;
    }
}
