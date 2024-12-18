using Domain.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Controllers
{
    public class OrderImporterControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        private readonly IResponseService _responseService;

        public OrderImporterControllerBase(ILogger<T> logger,
            IResponseService responseService)
        {
            _logger = logger;
            _responseService = responseService;
        }

        protected void LogInfo(string message)
        {
            string controllerName = $"{ControllerContext.ActionDescriptor.ControllerName}Controller";
            string actionName = ControllerContext.ActionDescriptor.ActionName;
            string messageInfo = $"{message}: {controllerName}, {actionName}";

            _logger.LogInformation(messageInfo);
        }

        protected void LogError(Exception ex, string message)
        {
            string controllerName = $"{ControllerContext.ActionDescriptor.ControllerName}Controller";
            string actionName = ControllerContext.ActionDescriptor.ActionName;

            string messageError = $"{message}: {controllerName}, {actionName}";
            _logger.LogError(ex, messageError);
        }

        protected ApiResponseKo ErrorResponse(string message, string trace,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            => _responseService.ErrorResponse(message, trace, GetErrorType(), httpStatusCode);

        private string GetErrorType()
        {
            string controllerName = ControllerContext.ActionDescriptor.ControllerName;
            return string.Format("OrderImporter.{0}", controllerName);
        }

    }
    
}
