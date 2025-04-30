using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Exceptions.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Exceptions.Mappers
{
    public class NotFoundExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception ex) => ex is NotFoundException;

        public int GetStatusCode() => StatusCodes.Status404NotFound;

        public object ToBasicError(Exception ex) => ex.Message;

        public ProblemDetails ToProblemDetails(Exception ex) => new()
        {
            Title = "NotFound",
            Detail = ex.Message,
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"
        };
    }
}
