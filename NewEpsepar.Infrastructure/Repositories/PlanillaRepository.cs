using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class PlanillaRepository : GenericRepository<Planilla>, IPlanillaRepository
    {
        public PlanillaRepository(EpseparDbContext context) : base(context) { }
    }
}