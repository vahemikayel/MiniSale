using AutoMapper;
using MediatR;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Product.Entity;
using MiniSale.Api.Models.Product.Response;
using MiniSale.DummyData;
using MiniSale.Generic.Repository.Repositories;
using MiniSale.Generic.Repository.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Commands.Informers
{
    public class ProductGenerateDummyDataCommand : BaseRequest<List<ProductReadModel>>
    {
        /// <summary>
        /// Ex. 50,0000
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// ex. 10
        /// </summary>
        public int PriceMin { get; set; }

        /// <summary>
        /// Ex. 5000
        /// </summary>
        public int PriceMax { get; set; }

        /// <summary>
        /// Ex. 1
        /// </summary>
        public int PluMin { get; set; }

        /// <summary>
        /// ex. 99999
        /// </summary>
        public int PluMax { get; set; }
    }

    public class ProductGenerateDummyDataCommandHandler : IRequestHandler<ProductGenerateDummyDataCommand, List<ProductReadModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ProductEModel, Guid> _prodRepo;

        public ProductGenerateDummyDataCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prodRepo = _unitOfWork.Repository<ProductEModel, Guid>();
        }

        public async Task<List<ProductReadModel>> Handle(ProductGenerateDummyDataCommand request, CancellationToken cancellationToken)
        {
            var dummyGen = new ProductDataGenerator(request.Count, request.PriceMin, request.PriceMax, request.PluMin, request.PluMax);
            var dummy = dummyGen.GenerateDummyData();
            var prods = _mapper.Map<List<ProductEModel>>(dummy);

            await _prodRepo.Remove(x => 1 == 1, cancellationToken);
            await _prodRepo.AddRangeAsync(prods, cancellationToken);

            return _mapper.Map<List<ProductReadModel>>(prods);
        }
    }
}
