using Microsoft.EntityFrameworkCore;
using NewEpsepar.Domain;

namespace NewEpsepar.Infrastructure
{
    public class EpseparDbContext : DbContext
    {
        public EpseparDbContext(DbContextOptions<EpseparDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración adicional si es necesario
        }
    }
}