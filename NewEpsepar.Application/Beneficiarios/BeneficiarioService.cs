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

        public async Task<Beneficiario?> GetByIdAsync(int id)
        {
            return await _beneficiarioRepository.GetByIdAsync(id);
        }
        // Métodos CRUD adicionales aquí...
    }
}
