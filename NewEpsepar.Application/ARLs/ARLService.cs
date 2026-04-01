using System.Collections.Generic;
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

        public async Task<IEnumerable<ARL>> GetAllAsync()
        {
            return await _arlRepository.GetAllAsync();
        }

        public async Task<ARL?> GetByIdAsync(int id)
        {
            return await _arlRepository.GetByIdAsync(id);
        }

        public async Task<ARL> CreateAsync(ARL entity)
        {
            return await _arlRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(ARL entity)
        {
            await _arlRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(ARL entity)
        {
            await _arlRepository.DeleteAsync(entity);
        }
    }
}
