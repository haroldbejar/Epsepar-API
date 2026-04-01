namespace NewEpsepar.Application.DTOs.Beneficiarios
{
    public class BeneficiarioDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Parentesco { get; set; }
        public int EmpleadoId { get; set; }
    }
}