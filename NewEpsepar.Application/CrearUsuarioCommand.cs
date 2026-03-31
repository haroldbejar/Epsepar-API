using NewEpsepar.Domain;
namespace NewEpsepar.Application;

public class CrearUsuarioCommand
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UsuarioService
{
    public Usuario CrearUsuario(CrearUsuarioCommand cmd)
    {
        // Aquí se podría agregar validación, lógica de negocio, etc.
        return new Usuario
        {
            Nombre = cmd.Nombre,
            Email = cmd.Email,
            Activo = true
        };
    }
}
