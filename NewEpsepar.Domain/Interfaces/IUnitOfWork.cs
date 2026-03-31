using System.Threading.Tasks;

namespace NewEpsepar.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}