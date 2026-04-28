namespace api.Utilities
{
    public static class Queries
    {
        #region Queries de Serviços
        public const string GET_ALL_SERVICOS = "SELECT id, nome, descricao, valor FROM tb_servico";
        public const string GET_SERVICO_BY_ID =
            "SELECT id, nome, descricao, valor FROM tb_servico WHERE id = $1";
        public const string DELETE_SERVICO_BY_ID = "DELETE FROM tb_servico WHERE id = $1";
        public const string INSERT_SERVICO =
            "INSERT INTO tb_servico (id, nome, descricao, valor) VALUES ($1, $2, $3, $4)";
        public const string UPDATE_SERVICO_UPSERT =
            @"INSERT INTO tb_servico (id, nome, descricao, valor)
            VALUES ($1, $2, $3, $4)
            ON CONFLICT (id) DO UPDATE
            SET
                nome = EXCLUDED.nome,
                descricao = EXCLUDED.descricao,
                valor = EXCLUDED.valor
            WHERE
                nome IS DISTINCT FROM EXCLUDED.nome
                OR descricao IS DISTINCT FROM EXCLUDED.descricao
                OR valor IS DISTINCT FROM EXCLUDED.valor";
        public const string UPDATE_SERVICO =
            @"UPDATE tb_servico SET nome=$2, descricao=$3, valor=$4
            WHERE id = $1
            AND (
                nome IS DISTINCT FROM $2
                OR descricao IS DISTINCT FROM $3
                OR valor IS DISTINCT FROM $4
            )";
        #endregion

        #region Queries de Clientes
        public const string GET_ALL_CLIENTES =
            "SELECT id, nome, telefone, email, cpf, dt_nasc FROM tb_cliente";
        #endregion
    }
}
