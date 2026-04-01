using Microsoft.EntityFrameworkCore;
using NewEpsepar.Domain;

namespace NewEpsepar.Infrastructure
{
    public class EpseparDbContext : DbContext
    {
        public EpseparDbContext(DbContextOptions<EpseparDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ARL> ARLs { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<EmpresaPrestadora> EmpresasPrestadoras { get; set; }
        public DbSet<Planilla> Planillas { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración adicional si es necesario
        }
    }
}