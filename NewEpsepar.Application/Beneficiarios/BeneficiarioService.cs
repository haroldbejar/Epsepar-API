using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.Beneficiarios
{
    public class BeneficiarioService
    {
        private readonly IBeneficiarioRepository _beneficiarioRepository;
        public BeneficiarioService(IBeneficiarioRepository beneficiarioRepository)
        {
            _beneficiarioRepository = beneficiarioRepository;
        }

        public async Task<IEnumerable<Beneficiario>> GetAllAsync()
        {
            return await _beneficiarioRepository.GetAllAsync();
        }

        public async Task<Beneficiario?> GetByIdAsync(int id)
        {
            return await _beneficiarioRepository.GetByIdAsync(id);
        }

        public async Task<Beneficiario> AddAsync(Beneficiario beneficiario)
        {
            return await _beneficiarioRepository.AddAsync(beneficiario);
        }

        public async Task UpdateAsync(Beneficiario beneficiario)
        {
            await _beneficiarioRepository.UpdateAsync(beneficiario);
        }

        public async Task DeleteAsync(Beneficiario beneficiario)
        {
            await _beneficiarioRepository.DeleteAsync(beneficiario);
        }
    }
}
