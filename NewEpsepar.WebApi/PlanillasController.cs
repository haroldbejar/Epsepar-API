using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NewEpsepar.Application.Planillas;
using NewEpsepar.Application.DTOs.Planillas;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlanillasController : ControllerBase
    {
        private readonly PlanillaService _planillaService;
        private readonly IMapper _mapper;

        public PlanillasController(PlanillaService planillaService, IMapper mapper)
        {
            _planillaService = planillaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanillaDto>>> GetAll()
        {
            var planillas = await _planillaService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<PlanillaDto>>(planillas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanillaDto>> GetById(int id)
        {
            var planilla = await _planillaService.GetByIdAsync(id);
            if (planilla == null) return NotFound();
            return Ok(_mapper.Map<PlanillaDto>(planilla));
        }

        [HttpPost]
        public async Task<ActionResult<PlanillaDto>> Create([FromBody] PlanillaDto dto)
        {
            var planilla = _mapper.Map<Domain.Planilla>(dto);
            var created = await _planillaService.AddAsync(planilla);
            var resultDto = _mapper.Map<PlanillaDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PlanillaDto dto)
        {
            var existing = await _planillaService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            var planilla = _mapper.Map(dto, existing);
            await _planillaService.UpdateAsync(planilla);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _planillaService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _planillaService.DeleteAsync(existing);
            return NoContent();
        }
    }
}
