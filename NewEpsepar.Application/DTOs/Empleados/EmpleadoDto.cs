namespace NewEpsepar.Application.DTOs.Empleados
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public int TipoDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
        public int ClienteId { get; set; }
    }
}