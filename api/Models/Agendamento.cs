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
        public Cliente Cliente { get; set; }

        [Required]
        [Guid]
        public Funcionario Funcionario { get; set; }

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
        public Servico[] Servicos { get; set; }

        [Required]
        [AllowedValues("agendado", "concluido", "cancelado")]
        public string Status { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "{0} deve ser um valor positivo!")]
        public double? ValorTotal { get; set; }

        public Agendamento(
            string id,
            Cliente cliente,
            Funcionario funcionario,
            DateTime dataAgendamento,
            DateTime? dataConclusao,
            Servico[] servicos,
            string status,
            double? valorTotal
        )
        {
            Id = id;
            Cliente = cliente;
            Funcionario = funcionario;
            DataAgendamento = dataAgendamento;
            DataConclusao = dataConclusao;
            Status = status;
            Servicos = servicos;
            ValorTotal = valorTotal;
        }

        // Aqui não vamos utilizar sobrecargas para deixar explícita a criação de um agendamento sem data de conclusão e valor total, já que isso é algo comum.
    }
}
