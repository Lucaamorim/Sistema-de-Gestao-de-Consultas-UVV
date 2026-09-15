using System.ComponentModel.DataAnnotations;

namespace SistemaGestaoConsultasUVV.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme sua senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare(nameof(Senha), ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
