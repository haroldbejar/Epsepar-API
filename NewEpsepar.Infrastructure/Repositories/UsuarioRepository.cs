using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(EpseparDbContext context) : base(context)
        {
        }
        // Si se requieren métodos adicionales específicos, agréguelos aquí.
    }
}
