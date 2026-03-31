using NewEpsepar.Domain;
namespace NewEpsepar.Application;

public class CrearUsuarioCommand
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

