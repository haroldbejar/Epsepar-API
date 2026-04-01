using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewEpsepar.Domain;

namespace NewEpsepar.Infrastructure
{
    public class EpseparIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public EpseparIdentityDbContext(DbContextOptions<EpseparIdentityDbContext> options) : base(options) { }

        // Puedes agregar DbSet para tus entidades adicionales si lo requieres

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Cambia el nombre de la tabla de IdentityUser para evitar conflicto con Usuarios
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable("AspNetUsers");
            });
            // Puedes personalizar otros nombres de tablas de Identity aquí si lo requieres
        }
    }
}
