using FluentValidation;

namespace NewEpsepar.Application.ARLs
{
    public class CreateARLCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Nit { get; set; } = string.Empty;
    }

}
