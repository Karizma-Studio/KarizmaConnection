using System;

namespace KarizmaConnection.Core.Exceptions
{
    [Serializable]
    public class ResponseException : Exception
    {
        public int Code { get; private set; }
        public string? Message { get; private set; }

        public ResponseException(int code, string? message) : base(message)
        {
            Code = code;
            Message = message;
        }
    }
}