using System;

namespace KarizmaConnection.Core.Exceptions
{
    [Serializable]
    public class ResponseException : Exception
    {
        public int Code { get; private set; }
        public override string? Message { get; }

        public ResponseException(int code, string? message) : base(message)
        {
            Code = code;
            Message = message;
        }
    }
}