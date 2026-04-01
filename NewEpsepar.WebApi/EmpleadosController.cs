using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NewEpsepar.Application.Empleados;
using NewEpsepar.Application.DTOs.Empleados;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;
        private readonly IMapper _mapper;

        public EmpleadosController(EmpleadoService empleadoService, IMapper mapper)
        {
            _empleadoService = empleadoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetAll()
        {
            var empleados = await _empleadoService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<EmpleadoDto>>(empleados));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetById(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return Ok(_mapper.Map<EmpleadoDto>(empleado));
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDto>> Create([FromBody] CreateEmpleadoCommand command)
        {
            var empleado = _mapper.Map<Domain.Empleado>(command);
            var created = await _empleadoService.AddAsync(empleado);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<EmpleadoDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateEmpleadoCommand command)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            // Mapear los campos del comando al empleado existente
            empleado.Nombres = command.Nombre;
            empleado.Apellidos = command.Apellido;
            empleado.TipoDocumento = command.TipoDocumento;
            empleado.Documento = command.NumeroDocumento;
            empleado.ClienteId = command.ClienteId;
            await _empleadoService.UpdateAsync(empleado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var empleado = await _empleadoService.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            await _empleadoService.DeleteAsync(empleado);
            return NoContent();
        }
    }
}
