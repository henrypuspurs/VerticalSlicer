using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace VerticalSlicer.WebApi;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var xmlDocs = currentAssembly.GetReferencedAssemblies()
            .Union(new[] { currentAssembly.GetName() })
            .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location)!, $"{a.Name}.xml"))
            .Where(File.Exists)
            .ToArray();

        Array.ForEach(xmlDocs, (d) => { options.IncludeXmlComments(d); });
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = "Vertical Slice template",
            Version = desc.ApiVersion.ToString()
        };

        if (desc.IsDeprecated)
        {
            info.Description +=
                "This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
}