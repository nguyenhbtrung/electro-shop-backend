using electro_shop_backend.Exceptions.Mappers.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Exceptions.Handlers
{
    public class ProblemDetailsExceptionHandler : IExceptionHandler
    {
        private readonly IEnumerable<IExceptionMapper> _mappers;
        private readonly ILogger<ProblemDetailsExceptionHandler> _logger;

        public ProblemDetailsExceptionHandler(IEnumerable<IExceptionMapper> mappers, ILogger<ProblemDetailsExceptionHandler> logger)
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

            var details = mapper is not null
                ? mapper.ToProblemDetails(exception)
                : new ProblemDetails
                {
                    Title = "Server error",
                    Detail = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError
                };

            httpContext.Response.StatusCode = details.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            return true;

        }
    }
}
