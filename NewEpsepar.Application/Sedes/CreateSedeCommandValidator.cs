using FluentValidation;

namespace NewEpsepar.Application.Sedes
{
    public class CreateSedeCommandValidator : AbstractValidator<CreateSedeCommand>
    {
        public CreateSedeCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.Direccion).NotEmpty().WithMessage("La dirección es obligatoria");
        }
    }
}
