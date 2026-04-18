
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace api.Models
{
    public class Servico
    {
        private string _nome;
        private string? _descricao;
        private double _preco;

        public string Nome
        {
            get { return _nome; }

            [MemberNotNull(nameof(_nome))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 3)
                {
                    throw new ArgumentException("Nome do serviço deve conter pelo menos 3 caracteres!");
                }

                _nome = value;
            }
        }

        public string? Descricao
        {
            get { return _descricao; }
            set
            {
                if (value == null)
                {
                    _descricao = null;
                    return;
                }

                if (value.Length < 5)
                {
                    throw new ArgumentException("Descrição deve conter pelo menos 5 caracteres!");
                }

                _descricao = value;
            }
        }

        public double Preco
        {
            get { return _preco; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Preço do serviço deve ser positivo!");
                }
                _preco = value;
            }
        }

        public Servico(string nome, string? descricao, double preco) {
            
                Nome = nome;
                Descricao = descricao;
                Preco = preco;
            }

        public Servico(string nome, double preco) : this(nome, null, preco) { }
    }
}
