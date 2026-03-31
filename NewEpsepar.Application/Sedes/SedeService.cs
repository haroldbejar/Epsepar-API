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

        public async Task<Sede?> GetByIdAsync(int id)
        {
            return await _sedeRepository.GetByIdAsync(id);
        }
        // Métodos CRUD adicionales aquí...
    }
}
