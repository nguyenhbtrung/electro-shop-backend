using electro_shop_backend.Exceptions.Mappers.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Exceptions.Handlers
{
    public class BasicExceptionHandler : IExceptionHandler
    {
        private readonly IEnumerable<IExceptionMapper> _mappers;
        private readonly ILogger<BasicExceptionHandler> _logger;

        public BasicExceptionHandler(IEnumerable<IExceptionMapper> mappers, ILogger<BasicExceptionHandler> logger)
        {
            _mappers = mappers;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
            var mapper = _mappers.FirstOrDefault(m => m.CanMap(exception));

            var statusCode = mapper is not null
                ? mapper.GetStatusCode()
                : StatusCodes.Status500InternalServerError;

            var error = mapper is not null
                ? mapper.ToBasicError(exception)
                : "An unexpected error occurred.";

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;

        }
    }
}
