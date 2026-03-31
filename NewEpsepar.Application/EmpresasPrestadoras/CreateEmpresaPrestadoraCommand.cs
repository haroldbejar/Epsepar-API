using FluentValidation;

namespace NewEpsepar.Application.EmpresasPrestadoras
{
    public class CreateEmpresaPrestadoraCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Nit { get; set; } = string.Empty;
    }

}
