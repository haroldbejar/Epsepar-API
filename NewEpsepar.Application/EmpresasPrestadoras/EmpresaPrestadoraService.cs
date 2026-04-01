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

        public async Task<IEnumerable<EmpresaPrestadora>> GetAllAsync()
        {
            return await _empresaPrestadoraRepository.GetAllAsync();
        }

        public async Task<EmpresaPrestadora?> GetByIdAsync(int id)
        {
            return await _empresaPrestadoraRepository.GetByIdAsync(id);
        }

        public async Task<EmpresaPrestadora> AddAsync(EmpresaPrestadora empresaPrestadora)
        {
            return await _empresaPrestadoraRepository.AddAsync(empresaPrestadora);
        }

        public async Task UpdateAsync(EmpresaPrestadora empresaPrestadora)
        {
            await _empresaPrestadoraRepository.UpdateAsync(empresaPrestadora);
        }

        public async Task DeleteAsync(EmpresaPrestadora empresaPrestadora)
        {
            await _empresaPrestadoraRepository.DeleteAsync(empresaPrestadora);
        }
    }
}
