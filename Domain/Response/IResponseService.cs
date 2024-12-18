using System.Net;

namespace Domain.Response
{
    public interface IResponseService
    {
        ApiResponseKo ErrorResponse(string message, string trace, string type,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError);
    }
}
