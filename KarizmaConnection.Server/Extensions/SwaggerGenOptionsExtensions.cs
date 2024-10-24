using KarizmaConnection.Server.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KarizmaConnection.Server.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static void AddKarizmaConnectionSwaggerDocs(this SwaggerGenOptions options)
    {
        options.DocumentFilter<SwaggerDocumentationFilter>();
    }
}