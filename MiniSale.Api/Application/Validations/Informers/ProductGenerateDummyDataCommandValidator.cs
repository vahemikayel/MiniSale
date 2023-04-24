using FluentValidation;
using MiniSale.Api.Application.Commands.Informers;

namespace MiniSale.Api.Application.Validations.Informers
{
    public class ProductGenerateDummyDataCommandValidator : CRUDCommandValidator<ProductGenerateDummyDataCommand>
    {
        public ProductGenerateDummyDataCommandValidator() : base()
        {
            RuleFor(c => c.Count)
                .GreaterThan(0)
                .LessThanOrEqualTo(100000);

            RuleFor(c => c.PriceMin)
                .GreaterThanOrEqualTo(10)
                .LessThanOrEqualTo(10000);

            RuleFor(c => c.PriceMax)
                .GreaterThan(x => x.PriceMin)
                .WithMessage("PriceMax Must be greather than PriceMin")
                .LessThanOrEqualTo(10000);

            RuleFor(x => x.PluMin)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000);

            RuleFor(x => x.PluMax)
                .GreaterThan(x => x.PluMin)
                .WithMessage("PLUMax Must be greather tahn PriceMin")
                .LessThanOrEqualTo(99999);

            RuleFor(x => x)
                .Must(x => x.PluMax - x.PluMin >= x.Count)
                .WithMessage(x => $"Difference PluMax and PluMin must be greather or equal than {x.Count}");
        }
    }
}
