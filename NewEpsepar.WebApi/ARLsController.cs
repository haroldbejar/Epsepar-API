using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewEpsepar.Application.ARLs;
using AutoMapper;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ARLsController : ControllerBase
    {
        private readonly ARLService _arlService;
        private readonly IMapper _mapper;

        public ARLsController(ARLService arlService, IMapper mapper)
        {
            _arlService = arlService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var arls = await _arlService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<NewEpsepar.Application.DTOs.ARLs.ARLDto>>(arls);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var arl = await _arlService.GetByIdAsync(id);
            if (arl == null)
                return NotFound();
            var dto = _mapper.Map<NewEpsepar.Application.DTOs.ARLs.ARLDto>(arl);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewEpsepar.Application.DTOs.ARLs.ARLDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var entity = _mapper.Map<NewEpsepar.Domain.ARL>(dto);
            var creado = await _arlService.CreateAsync(entity);
            var creadoDto = _mapper.Map<NewEpsepar.Application.DTOs.ARLs.ARLDto>(creado);
            return CreatedAtAction(nameof(GetById), new { id = creadoDto.Id }, creadoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewEpsepar.Application.DTOs.ARLs.ARLDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El id de la URL no coincide con el del DTO.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existente = await _arlService.GetByIdAsync(id);
            if (existente == null)
                return NotFound();
            var entity = _mapper.Map<NewEpsepar.Domain.ARL>(dto);
            await _arlService.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _arlService.GetByIdAsync(id);
            if (existente == null)
                return NotFound();
            await _arlService.DeleteAsync(existente);
            return NoContent();
        }
    }
}
