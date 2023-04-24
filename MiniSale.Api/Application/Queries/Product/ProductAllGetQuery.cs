using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Api.Models.Product.Response;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Queries.Product
{
    public class ProductAllGetQuery : BaseQueryRequest<List<ProductReadModel>>
    {
    }

    public class ProductAllGetQueryHandler : IRequestHandler<ProductAllGetQuery, List<ProductReadModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductAllGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();
        }

        public async Task<List<ProductReadModel>> Handle(ProductAllGetQuery request, CancellationToken cancellationToken)
        {
            var res = await _prodRepo.GetAll()
                                     .AsNoTracking()
                                     .ToListAsync(cancellationToken);
            return _mapper.Map<List<ProductReadModel>>(res);
        }
    }
}
