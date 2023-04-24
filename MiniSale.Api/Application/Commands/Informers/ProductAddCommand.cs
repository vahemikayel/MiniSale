using AutoMapper;
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
    public class ProductAddCommand : ProductBaseCommand
    {
        
    }

    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, ProductReadModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductAddCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();
        }

        public async Task<ProductReadModel> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            var prodEntity = _mapper.Map<ProductEModel>(request);
            prodEntity.Id = Guid.NewGuid();
            await _prodRepo.AddAsync(prodEntity, cancellationToken);
            return _mapper.Map<ProductReadModel>(prodEntity);
        }
    }
}
