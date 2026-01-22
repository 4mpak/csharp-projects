using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Lab3.Task2.HttpGateway.Middleware;

public class MyMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (RpcException e)
        {
            context.Response.StatusCode = e.StatusCode switch
            {
                StatusCode.OK => StatusCodes.Status200OK,
                StatusCode.Aborted or StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
                StatusCode.Cancelled => StatusCodes.Status499ClientClosedRequest,
                StatusCode.DataLoss => StatusCodes.Status500InternalServerError,
                StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
                StatusCode.NotFound => StatusCodes.Status404NotFound,
                StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
                StatusCode.Unimplemented => StatusCodes.Status501NotImplemented,
                StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
                StatusCode.DeadlineExceeded => StatusCodes.Status504GatewayTimeout,
                StatusCode.Internal => StatusCodes.Status500InternalServerError,
                StatusCode.Unavailable => StatusCodes.Status503ServiceUnavailable,
                StatusCode.Unknown => StatusCodes.Status500InternalServerError,
                StatusCode.ResourceExhausted => StatusCodes.Status429TooManyRequests,
                StatusCode.FailedPrecondition => StatusCodes.Status412PreconditionFailed,
                StatusCode.OutOfRange => StatusCodes.Status416RangeNotSatisfiable,
                _ => StatusCodes.Status500InternalServerError,
            };
        }
    }
}