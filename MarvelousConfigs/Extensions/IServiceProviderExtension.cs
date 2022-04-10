using MarvelousConfigs.API.RMQ.Producers;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.DAL.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

namespace MarvelousConfigs.API.Extensions
{
    public static class IServiceProviderExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMicroservicesService, MicroservicesService>();
            services.AddScoped<IConfigsService, ConfigsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRequestClient, AuthRequestClient>();
            services.AddScoped<IMarvelousConfigsProducer, MarvelousConfigsProducer>();
            services.AddTransient<IMemoryCacheExtentions, MemoryCacheExtentions>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMicroserviceRepository, MicroservicesRepository>();
            services.AddScoped<IConfigsRepository, ConfigsRepository>();
        }

        public static void SetMemoryCache(this WebApplication app)
        {
            app.Services.CreateScope().ServiceProvider.GetRequiredService<IMemoryCacheExtentions>().SetMemoryCache();
        }

        public static void RegisterLogger(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            service.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog(config);
            });
        }

        public static void AddCustomAuth(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // указывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // Cтрока, представляющая издателя
                        ValidIssuer = AuthOptions.Issuer,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.Audience,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddAuthorization();
        }

        public static void AddMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) => { });
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyAPI",
                    Version = "v1"
                });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
                });
            });
        }

    }
}
