using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using HIDE.RestAndRant.Common.Consts;

namespace HIDE.RestAndRant.Web_Api.Helpers;

/// <summary>
/// Помощник для работы со Swagger
/// </summary>
/// <summary>
/// Конфигурация для Swagger
/// </summary>
/// <remarks>
/// Конфигурация для Swagger
/// </remarks>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    /// <summary>
    /// Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(name: description.GroupName, info: CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options: options);
    }

    /// <summary>
    /// Create information about the version of the API
    /// </summary>
    /// <param name="desc"></param>
    /// <returns>Information about the API</returns>
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        OpenApiInfo info = new()
        {
            Title = $"Api_RestAndRant",
            Version = desc.ApiVersion.ToString(),
            Description = ""
        };

        if (desc.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}
