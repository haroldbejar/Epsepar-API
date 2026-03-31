using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.Planillas
{
    public class PlanillaService
    {
        private readonly IPlanillaRepository _planillaRepository;
        public PlanillaService(IPlanillaRepository planillaRepository)
        {
            _planillaRepository = planillaRepository;
        }

        public async Task<Planilla?> GetByIdAsync(int id)
        {
            return await _planillaRepository.GetByIdAsync(id);
        }
        // Métodos CRUD adicionales aquí...
    }
}
