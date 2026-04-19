using api.Utilities;

namespace api.Models
{
    public abstract class Pessoa
    {
        public string Id
        {
            get;

            init
            {
                ArgumentNullException.ThrowIfNull(value);

                if (!Validators.IsValidUUID(value))
                {
                    throw new ArgumentException("ID deve ser um UUID válido!");
                }

                field = value;
            }
        }

        public string Nome
        {
            get;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 3)
                {
                    throw new ArgumentException("Nome deve conter pelo menos 3 caracteres!");
                }

                field = value;
            }
        }

        public string Telefone
        {
            get;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (!Validators.IsValidPhoneNumber(value))
                {
                    throw new ArgumentException("Telefone inválido! Deve conter 11 dígitos (sem o código do país) ou 13 dígitos (com o código do país '55').");
                }

                field = value;
            }
        }

        public string? CPF
        {
            get;
            set
            {
                if (value == null)
                {
                    field = null;
                    return;
                }

                // Remover pontuação
                value = value.Replace(".", "").Replace("-", "");

                if (!Validators.IsValidCPF(value))
                {
                    throw new ArgumentException("CPF inválido!");
                }

                field = value;
            }
        }

        public string? Email
        {
            get;
            set
            {
                if (value == null)
                {
                    field = null;
                    return;
                }

                if (!Validators.IsValidEmail(value))
                {
                    throw new ArgumentException("Email inválido!");
                }

                field = value;
            }
        }

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

        public Pessoa(string nome, string telefone) : this(null, nome, telefone, null, null) { }

        public Pessoa(string nome, string telefone, string cpf, string email) : this(null, nome, telefone, cpf, email) { }
    }
}
