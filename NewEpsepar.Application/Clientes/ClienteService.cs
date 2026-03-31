using NewEpsepar.Domain;
using NewEpsepar.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewEpsepar.Application.Clientes
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente> CreateAsync(CreateClienteCommand command)
        {
            var cliente = new Cliente
            {
                RazonSocial = command.RazonSocial,
                Nit = command.Nit,
                TipoCliente = (TipoClienteEnum)command.TipoCliente,
                Direccion = command.Direccion,
                Telefono = command.Telefono,
                Email = command.Email,
                Estado = true,
                FechaRegistro = System.DateTime.UtcNow
            };
            await _clienteRepository.AddAsync(cliente);
            return cliente;
        }

        public async Task UpdateAsync(int id, CreateClienteCommand command)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return;
            cliente.RazonSocial = command.RazonSocial;
            cliente.Nit = command.Nit;
            cliente.TipoCliente = (TipoClienteEnum)command.TipoCliente;
            cliente.Direccion = command.Direccion;
            cliente.Telefono = command.Telefono;
            cliente.Email = command.Email;
            await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente != null)
            {
                await _clienteRepository.DeleteAsync(cliente);
            }
        }
    }
}