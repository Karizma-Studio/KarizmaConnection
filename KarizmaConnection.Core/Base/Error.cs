using KarizmaConnection.Core.Exceptions;

namespace KarizmaConnection.Core.Base
{
    public class Error
    {
        public int Code { get; private set; }
        public string? Message { get; private set; }

        public Error(int code, string? message)
        {
            Code = code;
            Message = message;
        }

        public Error(ResponseException responseException)
        {
            Code = responseException.Code;
            Message = responseException.Message;
        }
    }
}