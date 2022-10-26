using System.Net;

namespace Application.Common.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string? message = null)
            : base((int)HttpStatusCode.Unauthorized, message ?? "Unauthorized Access.") { }
    }
}