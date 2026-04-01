using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewEpsepar.Application.Clientes;
using NewEpsepar.Application.DTOs.Clientes;
using AutoMapper;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClientesController(ClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
                return NotFound();
            var clienteDto = _mapper.Map<ClienteDto>(cliente);
            return Ok(clienteDto);
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var creado = await _clienteService.CreateAsync(command);
            var clienteDto = _mapper.Map<ClienteDto>(creado);
            return CreatedAtAction(nameof(GetById), new { id = clienteDto.Id }, clienteDto);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateClienteCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existente = await _clienteService.GetByIdAsync(id);
            if (existente == null)
                return NotFound();
            await _clienteService.UpdateAsync(id, command);
            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _clienteService.GetByIdAsync(id);
            if (existente == null)
                return NotFound();
            await _clienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
