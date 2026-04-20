using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using api.Utilities;

namespace api.Models
{
    public class Agendamento
    {
        [Required]
        [Guid]
        public string Id { get; init; }

        [Required]
        [Guid]
        public string IdCliente { get; set; }

        [Required]
        [Guid]
        public string IdFuncionario { get; set; }

        [Required]
        [DateTime]
        public DateTime DataAgendamento { get; set; }

        public DateTime? DataConclusao
        {
            get;
            set
            {
                if (value == null)
                {
                    field = value;
                    return;
                }

                if (value < DataAgendamento)
                {
                    throw new ArgumentException(
                        "DataConclusao não pode ser anterior à data de agendamento."
                    );
                }

                field = value;
            }
        }

        [Required]
        public string[] IdServicos { get; set; }

        [Required]
        [AllowedValues("agendado", "concluido", "cancelado")]
        public string Status { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double ValorTotal { get; set; }

        public Agendamento(
            string id,
            string idCliente,
            string idFuncionario,
            DateTime dataAgendamento,
            DateTime? dataConclusao,
            string status,
            double valorTotal
        )
        {
            Id = id;
            IdCliente = idCliente;
            IdFuncionario = idFuncionario;
            DataAgendamento = dataAgendamento;
            DataConclusao = dataConclusao;
            Status = status;
            ValorTotal = valorTotal;
        }

        // Aqui vamos utilizar sobrecargas para deixar explícita a criação de um agendamento sem data de conclusão e valor total, já que isso é algo comum.
    }
}
