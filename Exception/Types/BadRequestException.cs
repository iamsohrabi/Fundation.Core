using System.Net;

namespace Fundation.Core.Exception.Types;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}
