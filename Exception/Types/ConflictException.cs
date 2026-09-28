using System.Net;

namespace Fundation.Core.Exception.Types;

public class ConflictException : CustomException
{
    public ConflictException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.Conflict;
    }
}
