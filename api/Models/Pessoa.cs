using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
    public interface IPessoaDTO
    {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
    }

    public abstract class Pessoa
    {
        [Required]
        [Guid]
        public Guid Id { get; init; }

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

        [EmailAddress]
        public string? Email { get; set; }

        public Pessoa(Guid? id, string nome, string telefone, string? cpf, string? email)
        {
            if (id == null)
            {
                Id = Guid.NewGuid();
            }
            else
            {
                Id = id.Value;
            }
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
            Email = email;
        }
    }
}
