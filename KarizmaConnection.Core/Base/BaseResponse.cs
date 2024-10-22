namespace KarizmaConnection.Core.Base
{
    public class BaseResponse<TResult>
    {
        public TResult Result { get; private set; }
        public BaseError? Error { get; private set; }
        public bool HasError => Error != null;
        public BaseResponse(TResult result, BaseError? error = null)
        {
            Result = result;
            Error = error;
        }
    }
}