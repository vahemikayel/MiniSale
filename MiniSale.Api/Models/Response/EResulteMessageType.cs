namespace MiniSale.Api.Models.Response
{
    public enum EResulteMessageType : int
    {
        Unknown = 0,
        Success = 1,
        CatchException,
        IsNotPasswordAvailable,
        IsNotAuthorized,
        InvalidContent,
        SqlConnectionFailed


    }
}
