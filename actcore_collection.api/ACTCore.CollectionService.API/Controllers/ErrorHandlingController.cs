using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ACTCore.CollectionService.API.Controllers
{
    [Route("ErrorHandling")]
    [ApiController]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorHandlingController : ControllerBase
    {
        [Route("ProcessError")]
        [HttpGet()]
        public IActionResult ProcessError([FromServices] IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                // custom logic
                var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                return Problem(
                    detail: feature?.Error.StackTrace,
                    title: feature?.Error.Message,
                    instance: hostEnvironment.EnvironmentName,
                    statusCode: StatusCodes.Status500InternalServerError
                    );
            }
            else
            {
                //return Problem();
                return Problem(
                    detail: "An error occurred while processing your request.",
                    title: "Internal Server Error",
                    instance: hostEnvironment.EnvironmentName,
                    statusCode: StatusCodes.Status500InternalServerError);

            }
        }
    }
}
