using KarizmaConnection.Server.Extensions;
using KarizmaConnection.Server.RequestHandler;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KarizmaConnection.Server.Swagger;

internal class SwaggerDocumentationFilter() : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var handlerActions = RequestHandlerRegistry.GetAllHandlerActions;

        foreach (var handlerAction in handlerActions)
        {
            var actionMethodInfo = handlerAction.ActionMethodInfo;

            // Add Action Input Parameters Type Document
            var actionMethodInputParams = actionMethodInfo.GetParameters();
            var docInputParameters = actionMethodInputParams.Select(param =>
                new OpenApiParameter
                {
                    Name = param.Name,
                    In = ParameterLocation.Query,
                    Schema = context.SchemaGenerator.GenerateSchema(param.ParameterType, context.SchemaRepository)
                }).ToList();

            // Add Action Output Parameter Type Document
            var outputType = actionMethodInfo.ReturnType.IsGenericTaskType()
                ? actionMethodInfo.ReturnType.GetGenericArguments()[0]
                : actionMethodInfo.ReturnType;

            if (outputType == typeof(Task))
                outputType = typeof(void);

            // Generate Tags
            var tags = new List<OpenApiTag>
            {
                new()
                {
                    Name = handlerAction.HandlerType.Name,
                }
            };

            // Create Operation
            var operation = new OpenApiOperation
            {
                Summary =
                    $"Handler: {handlerAction.HandlerType.Name} | Action: {handlerAction.ActionMethodInfo.Name}",

                Description =
                    $"<p><b>Input Type(s):</b> {(actionMethodInputParams.Length > 0
                        ? string.Join(", ", actionMethodInputParams.Select(paramInfo => paramInfo.ParameterType).ToList())
                        : "No Input")}</p>"
                    + $"<p><b>Output Type:</b> {(outputType == typeof(void) ? "No Response, You should use Send Method." : outputType)}</p>",

                Parameters = docInputParameters,
                Tags = tags,
                Responses = outputType == typeof(void)
                    ? new OpenApiResponses
                    {
                        ["200"] = new()
                        {
                            Description = "No Response"
                        }
                    }
                    : new OpenApiResponses
                    {
                        ["200"] = new()
                        {
                            Description = "OK",
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                [outputType.Name] = new()
                                {
                                    Schema =
                                        context.SchemaGenerator
                                            .GenerateSchema(outputType, context.SchemaRepository)
                                }
                            }
                        }
                    }
            };

            //Add Operation to Swagger Docs
            swaggerDoc.Paths.Add(handlerAction.Address, new OpenApiPathItem
            {
                Operations = { [OperationType.Get] = operation }
            });
        }
    }
}