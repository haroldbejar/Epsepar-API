using FluentValidation;

namespace NewEpsepar.Application.ARLs
{
    public class CreateARLCommandValidator : AbstractValidator<CreateARLCommand>
    {
        public CreateARLCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.Nit).NotEmpty().WithMessage("El NIT es obligatorio");
        }
    }
}
