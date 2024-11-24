using System;

namespace KarizmaPlatform.Connection.Core.Exceptions
{
    [Serializable]
    public class ResponseException : Exception
    {
        public int Code { get; private set; }
        public override string? Message { get; }

        public ResponseException(int code, string? message = null) : base(message)
        {
            Code = code;
            Message = message;
        }
    }
}