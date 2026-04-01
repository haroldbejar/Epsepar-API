namespace NewEpsepar.Application.DTOs.Clientes
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Nit { get; set; }
        public string TipoCliente { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}