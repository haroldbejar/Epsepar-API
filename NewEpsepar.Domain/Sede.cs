using System;

namespace NewEpsepar.Domain
{
    public class Sede
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}