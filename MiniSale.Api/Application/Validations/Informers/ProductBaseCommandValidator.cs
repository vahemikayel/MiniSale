using FluentValidation;
using MiniSale.Api.Application.Commands.Informers;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Validations.Informers
{
    public class ProductBaseCommandValidator<T> : CRUDCommandValidator<T> where T : ProductBaseCommand, new()
    {
        public ProductBaseCommandValidator() : base()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(15);

            RuleFor(x => x.BarCode)
                .NotEmpty()
                .Length(13)
                .When(x => x != null);
        }
    }
}
