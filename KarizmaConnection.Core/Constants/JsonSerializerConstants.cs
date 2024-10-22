using System.Text.Json;

namespace KarizmaConnection.Core.Constants
{
    public static class JsonSerializerConstants
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true };
    }
}