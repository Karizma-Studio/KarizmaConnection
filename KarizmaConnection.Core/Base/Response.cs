namespace KarizmaConnection.Core.Base
{
    public class Response<TResult>
    {
        public TResult Result { get; private set; }
        public Error? Error { get; private set; }
        public bool HasError => Error != null;

        public Response(TResult result, Error? error = null)
        {
            Result = result;
            Error = error;
        }
    }
}