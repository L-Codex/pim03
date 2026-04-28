using api.Utilities;

namespace api.Models
{
    public struct ClienteDTO : IPessoaDTO
    {
        public Guid Id { get; init; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
        public DateTime? DtNascimento { get; set; }

        public ClienteDTO(
            Guid id,
            string nome,
            string telefone,
            string? cpf,
            string? email,
            DateTime? dtNascimento
        )
        {
            Id = id;
            Nome = nome;
            Telefone = telefone;
            CPF = cpf;
            Email = email;
            DtNascimento = dtNascimento;
        }
    }

    public class Cliente : Pessoa
    {
        [BirthDate(MaximumAge = 120)]
        public DateOnly? DtNascimento { get; set; }

        public Cliente(
            Guid? id,
            string nome,
            string telefone,
            string? cpf,
            string? email,
            DateOnly? dtNascimento
        )
            : base(id, nome, telefone, cpf, email)
        {
            DtNascimento = dtNascimento;
        }
    }
}
