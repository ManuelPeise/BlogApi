using Microsoft.OpenApi;

namespace Web.Api.Bundels
{
    public static class Swagger
    {
        public static void RegisterSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dummy API",
                    Version = "v1",
                    Description = "Dummy Api for some projects",
                    Contact = new OpenApiContact
                    {
                        Name = "Manuel Peise",
                        Email = "manuel.p80@gmx.de"
                    }
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = new List<string>()
                });
            });
        }
    }
}
