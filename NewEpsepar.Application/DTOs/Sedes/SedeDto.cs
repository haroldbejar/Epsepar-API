namespace NewEpsepar.Application.DTOs.Sedes
{
    public class SedeDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int ClienteId { get; set; }
    }
}