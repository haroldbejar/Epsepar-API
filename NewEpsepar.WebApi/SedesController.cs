using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NewEpsepar.Application.Sedes;
using NewEpsepar.Application.DTOs.Sedes;
using NewEpsepar.Domain;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SedesController : ControllerBase
    {
        private readonly SedeService _sedeService;
        private readonly IMapper _mapper;

        public SedesController(SedeService sedeService, IMapper mapper)
        {
            _sedeService = sedeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SedeDto>>> GetAll()
        {
            var sedes = await _sedeService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SedeDto>>(sedes));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SedeDto>> GetById(int id)
        {
            var sede = await _sedeService.GetByIdAsync(id);
            if (sede == null) return NotFound();
            return Ok(_mapper.Map<SedeDto>(sede));
        }

        [HttpPost]
        public async Task<ActionResult<SedeDto>> Create([FromBody] CreateSedeCommand command)
        {
            var sede = _mapper.Map<Sede>(command);
            var created = await _sedeService.AddAsync(sede);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SedeDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateSedeCommand command)
        {
            var existing = await _sedeService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            _mapper.Map(command, existing);
            await _sedeService.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _sedeService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _sedeService.DeleteAsync(existing);
            return NoContent();
        }
    }
}
