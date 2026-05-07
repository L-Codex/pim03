using api.Utilities;

namespace api.Models
{
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
