using KarizmaPlatform.Connection.Core.Exceptions;

namespace KarizmaPlatform.Connection.Core.Base
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