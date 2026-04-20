using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
    public abstract class Pessoa
    {
        [Required]
        [Guid]
        public string Id { get; init; }

        [Required]
        [MinLength(3, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string Nome { get; set; }

        [Required]
        [RegularExpression(
            @"55\d{11}",
            ErrorMessage = "{0} deve conter 13 dígitos, incluindo o código do país '55'."
        )]
        public string Telefone { get; set; }

        [CPF(AllowPunctuation = false)]
        public string? CPF { get; set; }

        [EmailAddress(ErrorMessage = "{0} deve ser um endereço de email válido!")]
        public string? Email { get; set; }

        public Pessoa(string? id, string nome, string telefone, string? cpf, string? email)
        {
            if (id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            else
            {
                Id = id;
            }
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
            Email = email;
        }

        public Pessoa(string nome, string telefone)
            : this(null, nome, telefone, null, null) { }

        public Pessoa(string nome, string telefone, string cpf, string email)
            : this(null, nome, telefone, cpf, email) { }
    }
}
