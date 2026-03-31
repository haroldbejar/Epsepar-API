using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class SedeRepository : GenericRepository<Sede>, ISedeRepository
    {
        public SedeRepository(EpseparDbContext context) : base(context) { }
    }
}