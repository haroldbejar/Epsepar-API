using FluentValidation;

namespace NewEpsepar.Application.Sedes
{
    public class CreateSedeCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }

}
