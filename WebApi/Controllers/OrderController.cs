using Microsoft.AspNetCore.Mvc;
using System.Net;
using Businnes.Interfaces;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using Domain.Response;
using Domain.Summary;
using Domain.Exceptions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("[controller]/v{version:apiVersion}")]
    [ApiController]
    public class OrderController : OrderImporterControllerBase<OrderController>
    {
        private readonly IOrderImporterService _orderImporterService;

        public OrderController(ILogger<OrderController> logger,
            IResponseService responseService,
            IOrderImporterService orderImporterService)
            : base(logger, responseService)
        {
            _orderImporterService = orderImporterService;
        }

        [HttpGet]
        [Route("GetOrders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(SummaryViewModel))]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Petición incorrecta", typeof(ApiResponseKo))]
        [SwaggerResponse(StatusCodes.Status424FailedDependency, "Ocurrió un error durante la ejecución de una petición externa", typeof(ApiResponseKo))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Ocurrió un error durante la ejecución de la petición", typeof(ApiResponseKo))]
        public async Task<IActionResult> Get()
        {
            LogInfo("Start process GetOrdersWithThreadsAsync");
            SummaryViewModel response;

            try
            {
                response = await _orderImporterService.GetOrdersWithThreadsAsync();
            }
            catch (OrderImporterFailedDependencyException oifdex)
            {
                LogError(oifdex, "Ocurrió un error durante la ejecución de una petición externa");
                return StatusCode(StatusCodes.Status424FailedDependency,
                    ErrorResponse("Ocurrió un error durante la importación de ordenes",
                    Guid.NewGuid().ToString(), HttpStatusCode.InternalServerError));
            }
            catch (Exception ex)
            {
                LogError(ex, "Ocurrió un error durante la ejecución de la petición");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorResponse("Ocurrió un error durante la ejecución de la petición",
                    Guid.NewGuid().ToString(), HttpStatusCode.InternalServerError));
            }

            LogInfo("End process GetOrdersWithThreadsAsync");

            return Ok(response);
        }
    }
}
