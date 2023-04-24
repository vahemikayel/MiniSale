namespace MiniSale.Api.Models.Response
{
    public class ResultModel<TData> : ResultModel
    {
        public TData Value { get; set; }

        public ResultModel() : base() { }
        public ResultModel(TData value, EResulteMessageType type)
        {
            MessageType = type;
            if (value != null &&
                type == EResulteMessageType.Success)
            {
                IsOk = true;
                Value = value;
            }

            if (value.ToString() == "" &&
               type != EResulteMessageType.Success)
                Value = value;
        }
    }

    public class ResultModel
    {
        public bool IsOk { get; set; }
        public EResulteMessageType MessageType { get; set; }
        public ResultModel() { }
    }
}
