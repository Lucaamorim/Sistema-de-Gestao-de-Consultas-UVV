using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SistemaGestaoConsultasUVV.Data;
using SistemaGestaoConsultasUVV.Models;

namespace SistemaGestaoConsultasUVV.Controllers
{
    // Protege TODAS as rotas deste controller: só usuários autenticados acessam.
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        private int UsuarioIdLogado =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Consultas
        public async Task<IActionResult> Index()
        {
            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == UsuarioIdLogado)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        // GET: /Consultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioIdLogado);

            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // GET: /Consultas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Especialidade,DataHora,Descricao")] Consulta consulta)
        {
            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consulta.UsuarioId = UsuarioIdLogado;
            _context.Add(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioIdLogado);

            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // POST: /Consultas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Especialidade,DataHora,Descricao,UsuarioId")] Consulta consulta)
        {
            if (id != consulta.Id) return NotFound();

            // Garante que o usuário só edite consultas que pertencem a ele.
            var pertenceAoUsuario = await _context.Consultas
                .AsNoTracking()
                .AnyAsync(c => c.Id == id && c.UsuarioId == UsuarioIdLogado);

            if (!pertenceAoUsuario) return Forbid();

            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consulta.UsuarioId = UsuarioIdLogado;

            try
            {
                _context.Update(consulta);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Consultas.AnyAsync(c => c.Id == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Consultas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioIdLogado);

            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // POST: /Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == UsuarioIdLogado);

            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
