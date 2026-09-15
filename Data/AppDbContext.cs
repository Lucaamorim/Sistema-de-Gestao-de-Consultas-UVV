using Microsoft.EntityFrameworkCore;
using SistemaGestaoConsultasUVV.Models;

namespace SistemaGestaoConsultasUVV.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // E-mail deve ser único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relacionamento 1:N entre Usuario e Consulta
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Consultas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
