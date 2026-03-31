using NewEpsepar.Domain.Interfaces;
using System.Threading.Tasks;

namespace NewEpsepar.Infrastructure.UnitOfWork
{
    public class EpseparUnitOfWork : IUnitOfWork
    {
        private readonly EpseparDbContext _context;

        public EpseparUnitOfWork(EpseparDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}