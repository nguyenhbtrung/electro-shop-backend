using electro_shop_backend.Exceptions.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Exceptions.Mappers
{
    public class ArgumentExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception ex) => ex is ArgumentException;

        public int GetStatusCode() => StatusCodes.Status400BadRequest;

        public object ToBasicError(Exception ex) => ex.Message;

        public ProblemDetails ToProblemDetails(Exception ex) => new()
        {
            Title = "BadRequest",
            Detail = ex.Message,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
        };
    }
}
