using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewEpsepar.Application.Usuarios;
using NewEpsepar.Application.DTOs.Usuarios;
using NewEpsepar.Domain;
using AutoMapper;

namespace NewEpsepar.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(UsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(usuariosDto);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return Ok(usuarioDto);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Map command a entidad Usuario
            var usuario = _mapper.Map<Usuario>(command);
            var creado = await _usuarioService.CrearUsuarioAsync(usuario);
            var usuarioDto = _mapper.Map<UsuarioDto>(creado);
            return CreatedAtAction(nameof(GetById), new { id = usuarioDto.Id }, usuarioDto);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUsuarioCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existente = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (existente == null)
                return NotFound();
            // Map command a entidad existente
            _mapper.Map(command, existente);
            await _usuarioService.UpdateUsuarioAsync(existente);
            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            await _usuarioService.DeleteUsuarioAsync(usuario);
            return NoContent();
        }
    }
}
