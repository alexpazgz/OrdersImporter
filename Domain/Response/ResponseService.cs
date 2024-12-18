using System.Net;

namespace Domain.Response
{
    public class ResponseService : IResponseService
    {
        public ApiResponseKo ErrorResponse(string message, string trace, string type,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            => CreateResponse(message, trace, type, httpStatusCode);

        #region Metodos privados

        private ApiResponseKo CreateResponse(string message, string trace, string type,
           HttpStatusCode httpStatusCode)
        {
            return new ApiResponseKo(message, trace, type, httpStatusCode);
        }

        #endregion
    }
}
