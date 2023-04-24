namespace MiniSale.Api.Infrastructure.BaseReuqestTypes
{
    public class BaseQueryRequest<TResponse> : BaseRequest<TResponse>
    {
        public BaseQueryRequest() : base(false, false, false, false)
        {

        }
    }
}
