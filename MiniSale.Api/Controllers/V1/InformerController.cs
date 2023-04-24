using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSale.Api.Application.Authentication;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSale.Api.Models.Product.Response;
using System.Collections.Generic;
using MiniSale.Api.Application.Commands.Informers;
using MiniSale.Api.Application.Commands.Account;

namespace MiniSale.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    public class InformerController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        public InformerController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = Policies.Operator)]
        public async Task<ActionResult<List<ProductReadModel>>> GenerateProductData([FromBody] ProductGenerateDummyDataCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return BadRequest("Product Dummy Data has not been created.");

            return Ok(result);
        }
    }
}
