using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Funcionario : Pessoa
    {
        [Required]
        [MinLength(5)]
        public string Endereco { get; set; }

        public Funcionario(
            string? id,
            string nome,
            string telefone,
            string cpf,
            string email,
            string endereco
        )
            : base(id, nome, telefone, cpf, email)
        {
            Endereco = endereco;
        }

        public Funcionario(string nome, string telefone, string cpf, string email, string endereco)
            : this(null, nome, telefone, cpf, email, endereco) { }
    }
}
