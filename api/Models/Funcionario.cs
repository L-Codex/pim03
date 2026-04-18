using System.Diagnostics.CodeAnalysis;

namespace api.Models
{
    public class Funcionario : Pessoa
    {
        private string _endereco;

        public string Endereco
        {
            get { return _endereco; }
            [MemberNotNull(nameof(_endereco))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 5)
                {
                    throw new ArgumentException("Endereço deve conter pelo menos 5 caracteres!");
                }

                _endereco = value;
            }
        }

        public Funcionario(string nome, string telefone, string cpf, string email, string endereco) : base(nome, telefone, cpf, email)
        {
            Endereco = endereco;
        }
    }
}
