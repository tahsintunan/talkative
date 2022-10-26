using System.Net;

namespace Application.Common.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string? message = null)
            : base((int)HttpStatusCode.BadRequest, message ?? "Bad Request.") { }
    }
}
