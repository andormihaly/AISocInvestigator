using Microsoft.AspNetCore.Mvc;

namespace AISocInvestigator.SecurityMcpServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Service = "AI SOC Investigator Security MCP Server",
            Status = "Running"
        });
    }
}