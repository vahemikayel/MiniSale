using FluentValidation;
using MiniSale.Api.Application.Commands.Informers;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MiniSale.Api.Application.Validations.Informers
{
    public class ProductAddCommandValidator : ProductBaseCommandValidator<ProductAddCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductAddCommandValidator(IUnitOfWork unitOfWork) : base()
        {
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();

            RuleFor(x => x)
                .MustAsync(NameNotExists)
                .WithMessage(c => $"A product whith the Name {c.Name} already exists")
                .MustAsync(BarCodeNotExists)
                .When(x => x.BarCode != null)
                .WithMessage(c => $"A product whith the BarCode {c.BarCode} already exists")
                .MustAsync(PLUNotExists)
                .WithMessage(c => $"A product whith the PLU {c.PLU} already exists");
        }

        private async Task<bool> NameNotExists(ProductAddCommand command, CancellationToken cancellationToken)
        {
            var res = await _prodRepo.Find(x => x.Name.ToLower().Trim() == command.Name.ToLower().Trim())
                                     .AnyAsync(cancellationToken);
            return !res;
        }

        private async Task<bool> BarCodeNotExists(ProductAddCommand command, CancellationToken cancellationToken)
        {
            var res = await _prodRepo.Find(x => x.BarCode.ToLower().Trim() == command.BarCode.ToLower().Trim())
                                     .AnyAsync(cancellationToken);
            return !res;
        }

        private async Task<bool> PLUNotExists(ProductAddCommand command, CancellationToken cancellationToken)
        {
            var res = await _prodRepo.Find(x => x.PLU == command.PLU)
                                     .AnyAsync(cancellationToken);
            return !res;
        }
    }
}
