using System;
using System.Text.Json.Serialization;

namespace KarizmaConnection.Core.Base
{
    public class BaseError
    {
        public int Code { get; private set; }
        public string? Message { get; private set; }
        
        public BaseError(int code, string? message)
        {
            Code = code;
            Message = message;
        }
    }
}