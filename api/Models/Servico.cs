using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
    public sealed class ServicoDTO
    {
        public string Id { get; init; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public double Preco { get; set; }

        public ServicoDTO(string id, string nome, string? descricao, double preco)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
        }
    }

    public class Servico
    {
        [Required]
        [Guid]
        public string Id { get; init; }

        [Required]
        [MinLength(3, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string Nome { get; set; }

        [MinLength(5, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, 9999.99, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double Preco { get; set; }

        public Servico(string? id, string nome, string? descricao, double preco)
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
            Descricao = descricao;
            Preco = preco;
        }

        public Servico(string id, string nome, double preco)
            : this(id, nome, null, preco) { }

        public Servico(string nome, double preco)
            : this(null, nome, null, preco) { }
    }
}
