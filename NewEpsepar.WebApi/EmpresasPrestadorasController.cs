using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NewEpsepar.Application.EmpresasPrestadoras;
using NewEpsepar.Application.DTOs.EmpresasPrestadoras;
using NewEpsepar.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpresasPrestadorasController : ControllerBase
    {
        private readonly EmpresaPrestadoraService _service;
        private readonly IMapper _mapper;

        public EmpresasPrestadorasController(EmpresaPrestadoraService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaPrestadoraDto>>> GetAll()
        {
            var entities = await _service.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<EmpresaPrestadoraDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpresaPrestadoraDto>> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            var dto = _mapper.Map<EmpresaPrestadoraDto>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaPrestadoraDto>> Create([FromBody] CreateEmpresaPrestadoraCommand command)
        {
            var entity = new EmpresaPrestadora
            {
                Nombre = command.Nombre,
                Nit = command.Nit,
                Estado = true
            };
            var created = await _service.AddAsync(entity);
            var dto = _mapper.Map<EmpresaPrestadoraDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateEmpresaPrestadoraCommand command)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            entity.Nombre = command.Nombre;
            entity.Nit = command.Nit;
            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            await _service.DeleteAsync(entity);
            return NoContent();
        }
    }
}
