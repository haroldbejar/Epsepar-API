using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.ARLs
{
    public class ARLService
    {
        private readonly IARLRepository _arlRepository;
        public ARLService(IARLRepository arlRepository)
        {
            _arlRepository = arlRepository;
        }

        public async Task<ARL?> GetByIdAsync(int id)
        {
            return await _arlRepository.GetByIdAsync(id);
        }
        // Métodos CRUD adicionales aquí...
    }
}
