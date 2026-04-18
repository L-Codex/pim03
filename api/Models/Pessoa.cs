using api.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace api.Models
{
    public abstract class Pessoa
    {
        private string _nome;
        private string _telefone;
        private string? _cpf;
        private string? _email;

        public string Nome
        {
            get { return _nome; }

            // https://stackoverflow.com/a/71702779
            [MemberNotNull(nameof(_nome))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length < 3)
                {
                    throw new ArgumentException("Nome deve conter pelo menos 3 caracteres!");
                }

                _nome = value;
            }
        }

        public string Telefone
        {
            get { return _telefone; }

            [MemberNotNull(nameof(_telefone))]
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (!Validators.IsValidPhoneNumber(value))
                {
                    throw new ArgumentException("Telefone inválido! Deve conter 11 dígitos (sem o código do país) ou 13 dígitos (com o código do país '55').");
                }

                _telefone = value;
            }
        }

        public string? CPF
        {
            get { return _cpf; }
            set
            {
                if (value == null)
                {
                    _cpf = null;
                    return;
                }

                // Remover pontuação
                value = value.Replace(".", "").Replace("-", "");

                if (!Validators.IsValidCPF(value))
                {
                    throw new ArgumentException("CPF inválido!");
                }

                _cpf = value;
            }
        }

        public string? Email
        {
            get { return _email; }
            set
            {
                if (value == null)
                {
                    _email = null;
                    return;
                }

                if (!Validators.IsValidEmail(value))
                {
                    throw new ArgumentException("Email inválido!");
                }

                _email = value;
            }
        }

        public Pessoa(string nome, string telefone, string? cpf, string? email)
        {
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
            Email = email;
        }
        public Pessoa(string nome, string telefone) : this(nome, telefone, null, null) { }
    }
}
