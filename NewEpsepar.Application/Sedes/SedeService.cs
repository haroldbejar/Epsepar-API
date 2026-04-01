using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.Sedes
{
    public class SedeService
    {
        private readonly ISedeRepository _sedeRepository;
        public SedeService(ISedeRepository sedeRepository)
        {
            _sedeRepository = sedeRepository;
        }

        public async Task<IEnumerable<Sede>> GetAllAsync()
        {
            return await _sedeRepository.GetAllAsync();
        }

        public async Task<Sede?> GetByIdAsync(int id)
        {
            return await _sedeRepository.GetByIdAsync(id);
        }

        public async Task<Sede> AddAsync(Sede sede)
        {
            return await _sedeRepository.AddAsync(sede);
        }

        public async Task UpdateAsync(Sede sede)
        {
            await _sedeRepository.UpdateAsync(sede);
        }

        public async Task DeleteAsync(Sede sede)
        {
            await _sedeRepository.DeleteAsync(sede);
        }
    }
}
