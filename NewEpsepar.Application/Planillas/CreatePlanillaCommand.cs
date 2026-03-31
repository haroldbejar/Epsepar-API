using FluentValidation;

namespace NewEpsepar.Application.Planillas
{
    public class CreatePlanillaCommand
    {
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }
    }

    public class CreatePlanillaCommandValidator : AbstractValidator<CreatePlanillaCommand>
    {
        public CreatePlanillaCommandValidator()
        {
            RuleFor(x => x.EmpleadoId).GreaterThan(0);
            RuleFor(x => x.Fecha).NotEmpty();
            RuleFor(x => x.Valor).GreaterThan(0);
        }
    }
}
