using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Servico
    {
        [Required]
        [MinLength(3, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string Nome { get; set; }

        [MinLength(5, ErrorMessage = "{0} deve conter pelo menos {1} caracteres!")]
        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double Preco { get; set; }

        public Servico(string nome, string? descricao, double preco)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
        }

        public Servico(string nome, double preco)
            : this(nome, null, preco) { }
    }
}
