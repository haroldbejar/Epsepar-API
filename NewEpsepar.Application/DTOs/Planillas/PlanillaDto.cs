namespace NewEpsepar.Application.DTOs.Planillas
{
    public class PlanillaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }
        public int EmpleadoId { get; set; }
    }
}