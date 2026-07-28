using AISocInvestigator.Application.Core;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace AISocInvestigator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMessageBus? _bus;

    protected IMessageBus Bus => _bus ??= HttpContext.RequestServices.GetRequiredService<IMessageBus>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}