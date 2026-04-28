using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Funcionario(
        Guid? id,
        string nome,
        string telefone,
        string cpf,
        string email,
        string endereco
    ) : Pessoa(id, nome, telefone, cpf, email)
    {
        [Required]
        [MinLength(5)]
        public string Endereco { get; set; } = endereco;
    }
}
