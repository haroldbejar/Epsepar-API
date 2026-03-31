using FluentValidation;

namespace NewEpsepar.Application.EmpresasPrestadoras
{
    public class CreateEmpresaPrestadoraCommandValidator : AbstractValidator<CreateEmpresaPrestadoraCommand>
    {
        public CreateEmpresaPrestadoraCommandValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.Nit).NotEmpty().WithMessage("El NIT es obligatorio");
        }
    }
}
