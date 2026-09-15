using System.ComponentModel.DataAnnotations;

namespace SistemaGestaoConsultasUVV.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        // Armazena o HASH da senha, nunca a senha em texto puro.
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(300)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Navegação: um usuário pode ter várias consultas
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
