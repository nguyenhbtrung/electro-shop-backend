using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Exceptions.Mappers.Interfaces
{
    public interface IExceptionMapper
    {
        bool CanMap(Exception ex);
        ProblemDetails ToProblemDetails(Exception ex);

    }
}
