﻿namespace MiniSale.Api.Infrastructure.BaseReuqestTypes
{
    public class BaseHttpRequest<TResponse> : BaseRequest<TResponse>
    {
        public BaseHttpRequest() : base(false, false, false, false)
        {

        }
    }
}
