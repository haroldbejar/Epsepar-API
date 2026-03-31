using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.EmpresasPrestadoras
{
    public class EmpresaPrestadoraService
    {
        private readonly IEmpresaPrestadoraRepository _empresaPrestadoraRepository;
        public EmpresaPrestadoraService(IEmpresaPrestadoraRepository empresaPrestadoraRepository)
        {
            _empresaPrestadoraRepository = empresaPrestadoraRepository;
        }

        public async Task<EmpresaPrestadora?> GetByIdAsync(int id)
        {
            return await _empresaPrestadoraRepository.GetByIdAsync(id);
        }
        // Métodos CRUD adicionales aquí...
    }
}
