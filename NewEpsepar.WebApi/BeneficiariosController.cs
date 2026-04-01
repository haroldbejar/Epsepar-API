using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NewEpsepar.Application.Beneficiarios;
using NewEpsepar.Application.DTOs.Beneficiarios;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BeneficiariosController : ControllerBase
    {
        private readonly BeneficiarioService _beneficiarioService;
        private readonly IMapper _mapper;

        public BeneficiariosController(BeneficiarioService beneficiarioService, IMapper mapper)
        {
            _beneficiarioService = beneficiarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeneficiarioDto>>> GetAll()
        {
            var beneficiarios = await _beneficiarioService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BeneficiarioDto>>(beneficiarios));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeneficiarioDto>> GetById(int id)
        {
            var beneficiario = await _beneficiarioService.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            return Ok(_mapper.Map<BeneficiarioDto>(beneficiario));
        }

        [HttpPost]

        public async Task<ActionResult<BeneficiarioDto>> Create([FromBody] CreateBeneficiarioCommand command)
        {
            // Mapear el comando a la entidad
            var beneficiario = new Domain.Beneficiario
            {
                Nombres = command.Nombre,
                Apellidos = command.Apellido,
                TipoDocumento = (Domain.TipoDocumentoEnum)command.TipoDocumento,
                Documento = command.NumeroDocumento,
                Parentesco = (Domain.ParentescoEnum)command.Parentesco,
                EmpleadoId = command.EmpleadoId
            };
            var creado = await _beneficiarioService.AddAsync(beneficiario);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, _mapper.Map<BeneficiarioDto>(creado));
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update(int id, [FromBody] CreateBeneficiarioCommand command)
        {
            var beneficiario = await _beneficiarioService.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            beneficiario.Nombres = command.Nombre;
            beneficiario.Apellidos = command.Apellido;
            beneficiario.TipoDocumento = (Domain.TipoDocumentoEnum)command.TipoDocumento;
            beneficiario.Documento = command.NumeroDocumento;
            beneficiario.Parentesco = (Domain.ParentescoEnum)command.Parentesco;
            beneficiario.EmpleadoId = command.EmpleadoId;
            await _beneficiarioService.UpdateAsync(beneficiario);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var beneficiario = await _beneficiarioService.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            await _beneficiarioService.DeleteAsync(beneficiario);
            return NoContent();
        }
    }
}


