using api.Utilities;

namespace api.Models
{
    public struct ClienteDTO(
        Guid id,
        string nome,
        string telefone,
        string? cpf,
        string? email,
        DateTime? dtNascimento
    ) : IPessoaDTO
    {
        public Guid Id { get; init; } = id;
        public string Nome { get; set; } = nome;
        public string Telefone { get; set; } = telefone;
        public string? CPF { get; set; } = cpf;
        public string? Email { get; set; } = email;
        public DateTime? DtNascimento { get; set; } = dtNascimento;
    }

    public class Cliente(
        Guid? id,
        string nome,
        string telefone,
        string? cpf,
        string? email,
        DateOnly? dtNascimento
    ) : Pessoa(id, nome, telefone, cpf, email)
    {
        [BirthDate(MaximumAge = 120)]
        public DateOnly? DtNascimento { get; set; } = dtNascimento;
    }
}
