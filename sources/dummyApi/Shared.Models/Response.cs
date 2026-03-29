namespace Shared.Models
{
    public class ResponseBase
    {
        public bool Success { get; set; }

    }

    public class Response<TModel> : ResponseBase where TModel : class
    {
        public TModel? Data { get; set; }
    }
}
