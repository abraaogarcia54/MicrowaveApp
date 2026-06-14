using System.Net;
using System.Text.Json;
using MicrowaveApp.Application.Interfaces;
using MicrowaveApp.Domain.Exceptions;

namespace MicrowaveApp.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionLogger _exceptionLogger;

    public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger)
    {
        _next = next;
        _exceptionLogger = exceptionLogger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException exception)
        {
            await WriteErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                exception.ErrorCode,
                exception.Message);
        }
        catch (Exception exception)
        {
            await _exceptionLogger.LogAsync(
                exception,
                context.TraceIdentifier,
                context.Request.Path,
                context.Request.Method,
                context.RequestAborted);

            await WriteErrorAsync(
                context,
                HttpStatusCode.InternalServerError,
                "UNEXPECTED_ERROR",
                "Ocorreu um erro inesperado. Tente novamente.");
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string errorCode,
        string message)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse(
            errorCode,
            message,
            context.TraceIdentifier);

        await JsonSerializer.SerializeAsync(context.Response.Body, response);
    }

    private sealed record ApiErrorResponse(string ErrorCode, string Message, string TraceId);
}
