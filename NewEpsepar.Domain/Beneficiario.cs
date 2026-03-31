using System;
using System.Collections.Generic;

namespace NewEpsepar.Domain
{
    public class Beneficiario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public ParentescoEnum Parentesco { get; set; }
        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }
    }
}