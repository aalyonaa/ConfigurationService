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
string logDirectory = builder.Configuration.GetValue<string>(_logDirectoryVariableName);
string conString = builder.Configuration.GetValue<string>(_connectionStringVariableName);

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
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterLogger(config);
builder.Services.AddMassTransit();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExeptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.SetMemoryCache();

app.Run();
