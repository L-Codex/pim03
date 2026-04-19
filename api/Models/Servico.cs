namespace api.Models
{
    public class Servico
    {
        public string Nome
        {
            get;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 3)
                {
                    throw new ArgumentException(
                        "Nome do serviço deve conter pelo menos 3 caracteres!"
                    );
                }

                field = value;
            }
        }

        public string? Descricao
        {
            get;
            set
            {
                if (value == null)
                {
                    field = null;
                    return;
                }

                if (value.Length < 5)
                {
                    throw new ArgumentException("Descrição deve conter pelo menos 5 caracteres!");
                }

                field = value;
            }
        }

        public double Preco
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

                field = value;
            }
        }

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
