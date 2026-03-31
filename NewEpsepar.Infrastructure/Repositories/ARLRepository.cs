using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class ARLRepository : GenericRepository<ARL>, IARLRepository
    {
        public ARLRepository(EpseparDbContext context) : base(context) { }
    }
}