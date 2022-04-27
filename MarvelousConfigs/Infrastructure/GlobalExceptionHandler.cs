using FluentValidation;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace MarvelousConfigs.API.Infrastructure
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
            }
            catch (BadGatewayException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadGateway, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (ConflictException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (ServiceUnavailableException ex)
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
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var exceptionModel = new ExceptionResponseModel { Code = (int)code, Message = message };
            var result = JsonSerializer.Serialize(exceptionModel);
            await context.Response.WriteAsync(result);
            _logger.LogError($"Eror {code} : {message}");
        }
    }
}
