using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
    public struct ServicoDTO(Guid id, string nome, string? descricao, double preco)
    {
        public Guid Id = id;
        public string Nome = nome;
        public string? Descricao = descricao;
        public double Preco = preco;
    }

    public struct ServicoCreateDTO
    {
        [Required(ErrorMessage = "{0} é obrigatório!")]
        [MinLength(3, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string Nome { get; set; }

        [MinLength(5, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório!")]
        [Range(0.01, 9999.99, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double Preco { get; set; }
    }

    public struct ServicoUpdateDTO
    {
        [MinLength(3, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string? Nome { get; set; }

        [MinLength(5, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string? Descricao { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double? Preco { get; set; }
    }

    public class Servico
    {
        [Required]
        [Guid]
        public Guid Id { get; init; }

        [Required]
        [MinLength(3)]
        public string Nome { get; set; }

        [MinLength(5)]
        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public double Preco { get; set; }

        public Servico(Guid? id, string nome, string? descricao, double preco)
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
            Descricao = descricao;
            Preco = preco;
        }
    }
}
