using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestaoConsultasUVV.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A especialidade deve ter no máximo {1} caracteres.")]
        [Display(Name = "Especialidade")]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        // Relacionamento com Usuario (chave estrangeira)
        [Required]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }
    }
}
