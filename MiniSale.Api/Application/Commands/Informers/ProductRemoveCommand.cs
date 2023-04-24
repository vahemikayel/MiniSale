using MediatR;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Api.Models.Product.Response;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Commands.Informers
{
    public class ProductRemoveCommand : BaseRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ProductRemoveCommandHandler : IRequestHandler<ProductRemoveCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductRemoveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();
        }

        public async Task<bool> Handle(ProductRemoveCommand request, CancellationToken cancellationToken)
        {
            await _prodRepo.Remove(x => x.Id == request.Id, cancellationToken);
            return true;
        }
    }
}
