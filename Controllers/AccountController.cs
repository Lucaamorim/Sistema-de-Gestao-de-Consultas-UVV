using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SistemaGestaoConsultasUVV.Data;
using SistemaGestaoConsultasUVV.Models;
using SistemaGestaoConsultasUVV.Models.ViewModels;

namespace SistemaGestaoConsultasUVV.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        // PasswordHasher garante que a senha nunca fique salva em texto puro no banco.
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
            if (emailJaExiste)
            {
                ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                DataCadastro = DateTime.Now
            };

            // Gera o hash da senha (nunca gravar a senha em texto puro)
            usuario.Senha = _passwordHasher.HashPassword(usuario, model.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Cadastro realizado com sucesso! Faça login para continuar.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            var resultadoVerificacao = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, model.Senha);
            if (resultadoVerificacao == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("Index", "Consultas");
        }

        // GET/POST: /Account/Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
