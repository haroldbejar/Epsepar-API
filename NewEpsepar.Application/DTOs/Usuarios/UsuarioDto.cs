namespace NewEpsepar.Application.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Estado { get; set; }
    }
}