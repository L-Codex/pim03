using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
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
