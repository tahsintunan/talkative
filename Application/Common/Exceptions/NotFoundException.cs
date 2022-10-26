using System.Net;

namespace Application.Common.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string? message = null)
            : base((int)HttpStatusCode.NotFound, message ?? "Resource Not Found.") { }
    }
}
