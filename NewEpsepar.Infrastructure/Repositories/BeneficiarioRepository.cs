using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Infrastructure.Repositories
{
    public class BeneficiarioRepository : GenericRepository<Beneficiario>, IBeneficiarioRepository
    {
        public BeneficiarioRepository(EpseparDbContext context) : base(context) { }
    }
}