using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Helper.Exceptions;
using System.Net;
using System.Text.Json;

namespace MarvelousConfigs.API.Infrastructure
{
    public class GlobalExeptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExeptionHandler> _logger;

        public GlobalExeptionHandler(RequestDelegate next, ILogger<GlobalExeptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CacheLoadingException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadGateway, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadGateway, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
        {
            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            _logger.LogError($"Eror {code} : {message}");
            await context.Response.WriteAsync(result);
        }
    }
}
