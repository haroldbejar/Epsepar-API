using System;
using System.Collections.Generic;

namespace NewEpsepar.Domain
{
    public enum TipoClienteEnum
    {
        Natural = 1,
        Juridico = 2
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Nit { get; set; }
        public TipoClienteEnum TipoCliente { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public ICollection<Empleado> Empleados { get; set; }
        public ICollection<Sede> Sedes { get; set; }
    }
}