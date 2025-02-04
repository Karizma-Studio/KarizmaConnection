using KarizmaPlatform.Connection.Server.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KarizmaPlatform.Connection.Server.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static void AddKarizmaConnectionSwaggerDocs(this SwaggerGenOptions options)
    {
        SwaggerDocumentationFilter.FindCustomEventDocs();
        options.DocumentFilter<SwaggerDocumentationFilter>();
    }
}