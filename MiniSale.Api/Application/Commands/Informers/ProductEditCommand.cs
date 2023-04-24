using AutoMapper;
using MediatR;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Api.Models.Product.Response;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace MiniSale.Api.Application.Commands.Informers
{
    public class ProductEditCommand : ProductBaseCommand
    {
        public Guid Id { get; set; }
    }

    public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, ProductReadModel>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductEditCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();
        }

        public async Task<ProductReadModel> Handle(ProductEditCommand request, CancellationToken cancellationToken)
        {
            var prodEntity = await _prodRepo.Find(x => x.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken);

            prodEntity = _mapper.Map<ProductEModel>(request);
            _prodRepo.Update(prodEntity);
            return _mapper.Map<ProductReadModel>(prodEntity);
        }
    }
}
