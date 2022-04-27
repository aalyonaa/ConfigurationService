using Marvelous.Contracts.Enums;
using MarvelousConfigs.API.Configuration;
using MarvelousConfigs.API.Extensions;
using MarvelousConfigs.API.Infrastructure;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.DAL.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(CustomMapperAPI).Assembly, typeof(CustomMapperBLL).Assembly);

string _connectionStringVariableName = "CONFIG_CONNECTION_STRING";
string _logDirectoryVariableName = "LOG_DIRECTORY";
string _authUrlVariableName = "IDENTITY_SERVICE_URL";
string logDirectory = builder.Configuration.GetValue<string>(_logDirectoryVariableName);
string conString = builder.Configuration.GetValue<string>(_connectionStringVariableName);
string authUrl = builder.Configuration.GetValue<string>(_authUrlVariableName);

builder.Services.Configure<DbConfiguration>(opt =>
{
    opt.ConnectionString = conString;
});

var config = new ConfigurationBuilder()
           .SetBasePath(logDirectory)
           .AddXmlFile("nlog.config", optional: true, reloadOnChange: true)
           .Build();

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

builder.Services.AddMemoryCache();
builder.Services.RegisterDependencies();
builder.Services.RegisterLogger(config);
builder.Services.AddMassTransit();
builder.Services.AddFluentValidation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

app.Configuration[Microservice.MarvelousAuth.ToString()] = authUrl;

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SetMemoryCache();

app.Run();
