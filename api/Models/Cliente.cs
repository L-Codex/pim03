namespace api.Models
{
    public class Cliente : Pessoa
    {
        public DateOnly? DtNascimento
        {
            get;
            set
            {
                if (value == null)
                {
                    field = null;
                    return;
                }

                if (value >= DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("Data de nascimento deve ser uma data passada!");
                }

                if (value <= DateOnly.FromDateTime(DateTime.Now).AddYears(-120))
                {
                    throw new ArgumentException(
                        "Data de nascimento deve ser uma data recente (menos de 120 anos atrás)!"
                    );
                }

                // TODO: Impor idade mínima (ex: 14 anos)
                field = value;
            }
        }

        public Cliente(
            string? id,
            string nome,
            string telefone,
            string? cpf,
            string? email,
            DateOnly? dtNascimento
        )
            : base(id, nome, telefone, cpf, email)
        {
            DtNascimento = dtNascimento;
        }

        public Cliente(
            string nome,
            string telefone,
            string cpf,
            string email,
            DateOnly dtNascimento
        )
            : this(null, nome, telefone, cpf, email, dtNascimento) { }

        public Cliente(string nome, string telefone)
            : this(null, nome, telefone, null, null, null) { }
    }
}
