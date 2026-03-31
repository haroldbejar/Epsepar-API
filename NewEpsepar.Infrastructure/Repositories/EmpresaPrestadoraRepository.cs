using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class EmpresaPrestadoraRepository : GenericRepository<EmpresaPrestadora>, IEmpresaPrestadoraRepository
    {
        public EmpresaPrestadoraRepository(EpseparDbContext context) : base(context) { }
    }
}