using api.Utilities;

namespace api.Models
{
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
