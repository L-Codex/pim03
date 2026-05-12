using System.ComponentModel.DataAnnotations;
using api.Utilities;

namespace api.Models
{
    public class Agendamento
    {
        [Required]
        [Guid]
        public Guid Id { get; init; }

        [Required]
        public Cliente Cliente { get; set; }

        [Required]
        [Guid]
        public Funcionario Funcionario { get; set; }

        [Required]
        [DateTime]
        public DateTime DataAgendamento { get; set; }

        [DateTime(AllowFuture = false, NotBefore = "DataAgendamento")]
        public DateTime? DataConclusao { get; set; }

        [Required]
        public Servico[] Servicos { get; set; }

        [Required]
        [AllowedValues("agendado", "concluido", "cancelado")]
        public string Status { get; set; }

        [Range(0.01, 9999.99)]
        public double? ValorTotal { get; set; }

        public Agendamento(
            Guid id,
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
    }
}
