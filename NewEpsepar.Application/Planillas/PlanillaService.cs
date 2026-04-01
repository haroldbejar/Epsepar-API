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

        public async Task<IEnumerable<Planilla>> GetAllAsync()
        {
            return await _planillaRepository.GetAllAsync();
        }

        public async Task<Planilla?> GetByIdAsync(int id)
        {
            return await _planillaRepository.GetByIdAsync(id);
        }

        public async Task<Planilla> AddAsync(Planilla planilla)
        {
            return await _planillaRepository.AddAsync(planilla);
        }

        public async Task UpdateAsync(Planilla planilla)
        {
            await _planillaRepository.UpdateAsync(planilla);
        }

        public async Task DeleteAsync(Planilla planilla)
        {
            await _planillaRepository.DeleteAsync(planilla);
        }
    }
}
