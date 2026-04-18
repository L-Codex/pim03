namespace api.Models
{
    public class Cliente : Pessoa
    {
        private DateOnly? _dt_nasc;

        public DateOnly? DtNascimento
        {
            get { return _dt_nasc; }
            set
            {
                if (value == null)
                {
                    _dt_nasc = null;
                    return;
                }

                if (value >= DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("Data de nascimento deve ser uma data passada!");
                }

                if (value <= DateOnly.FromDateTime(DateTime.Now).AddYears(-120))
                {
                    throw new ArgumentException("Data de nascimento deve ser uma data recente (menos de 120 anos atrás)!");
                }

                // TODO: Impor idade mínima (ex: 14 anos)
                _dt_nasc = value;
            }
        }


        public Cliente(string nome, string telefone, string? cpf, string? email, DateOnly? dtNascimento) : base(nome, telefone, cpf, email) { DtNascimento = dtNascimento; }

        public Cliente(string nome, string telefone) : this(nome, telefone, null, null, null) { }
    }
}
