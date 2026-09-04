using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ACTCore.CollectionService.API
{
    public class ConfiguerSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _configuration;

        public ConfiguerSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n " +
                        "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            // Read version info from version.json only
            var apiInfoTitle = _configuration.GetValue<string>("Title") ?? "ACTCore Collection API";
            var apiInfoVersion = _configuration.GetValue<string>("ReleaseVersion") ?? "1.0.0";
            var apiInfoBuild = _configuration.GetValue<string>("ReleaseBuild") ?? "";

            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Version = desc.ApiVersion.ToString(),
                    Title = $"{apiInfoTitle} Version {apiInfoVersion} Build {apiInfoBuild}",
                    //Description = "API to manage Security",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contract = new OpenApiContract
                    //{
                    //    Name = "ACTCore Security",
                    //    Url = new Uri("https://example.com/Contract")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "ACTCore License",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
            }
        }
    }
}
