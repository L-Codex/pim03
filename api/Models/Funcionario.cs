namespace api.Models
{
    public class Funcionario : Pessoa
    {

        public string Endereco
        {
            get;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 5)
                {
                    throw new ArgumentException("Endereço deve conter pelo menos 5 caracteres!");
                }

                field = value;
            }
        }

        public Funcionario(string? id, string nome, string telefone, string cpf, string email, string endereco) : base(id, nome, telefone, cpf, email)
        {
            Endereco = endereco;
        }

        public Funcionario(string nome, string telefone, string cpf, string email, string endereco) : this(null, nome, telefone, cpf, email, endereco) { }
    }
}
