using FluentValidation;

namespace NewEpsepar.Application.Clientes
{
    public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteCommandValidator()
        {
            RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Nit).NotEmpty().MaximumLength(20);
            RuleFor(x => x.TipoCliente).IsInEnum();
            RuleFor(x => x.Direccion).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}