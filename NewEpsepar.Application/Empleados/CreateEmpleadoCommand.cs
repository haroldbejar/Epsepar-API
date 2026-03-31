using FluentValidation;

namespace NewEpsepar.Application.Empleados
{
    public class CreateEmpleadoCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }

    public class CreateEmpleadoCommandValidator : AbstractValidator<CreateEmpleadoCommand>
    {
        public CreateEmpleadoCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apellido).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TipoDocumento).IsInEnum();
            RuleFor(x => x.NumeroDocumento).NotEmpty().MaximumLength(20);
            RuleFor(x => x.ClienteId).GreaterThan(0);
        }
    }
}
