using System.Threading.Tasks;
using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;

namespace NewEpsepar.Application.Empleados
{
    public class EmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        public EmpleadoService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _empleadoRepository.GetAllAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _empleadoRepository.GetByIdAsync(id);
        }

        public async Task<Empleado> AddAsync(Empleado empleado)
        {
            return await _empleadoRepository.AddAsync(empleado);
        }

        public async Task UpdateAsync(Empleado empleado)
        {
            await _empleadoRepository.UpdateAsync(empleado);
        }

        public async Task DeleteAsync(Empleado empleado)
        {
            await _empleadoRepository.DeleteAsync(empleado);
        }
    }
}
