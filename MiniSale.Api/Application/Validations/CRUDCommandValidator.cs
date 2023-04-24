using FluentValidation;

namespace MiniSale.Api.Application.Validations
{
    public class CRUDCommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : new()
    {
        public CRUDCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
