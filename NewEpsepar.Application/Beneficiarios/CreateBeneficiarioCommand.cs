using FluentValidation;

namespace NewEpsepar.Application.Beneficiarios
{
    public class CreateBeneficiarioCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public int Parentesco { get; set; }
        public int EmpleadoId { get; set; }
    }

    public class CreateBeneficiarioCommandValidator : AbstractValidator<CreateBeneficiarioCommand>
    {
        public CreateBeneficiarioCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apellido).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TipoDocumento).IsInEnum();
            RuleFor(x => x.NumeroDocumento).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Parentesco).IsInEnum();
            RuleFor(x => x.EmpleadoId).GreaterThan(0);
        }
    }
}
