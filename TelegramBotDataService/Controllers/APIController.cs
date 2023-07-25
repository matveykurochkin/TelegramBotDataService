using Microsoft.AspNetCore.Mvc;
using NLog;

namespace TelegramBotDataService.Controllers;

[Route("api")]
[ApiController]
// ReSharper disable once InconsistentNaming
public class APIController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    [HttpPost("GetLogFile")]
    public Task<IActionResult> GetLogFile(DateTime date, CancellationToken cancellationToken)
    {
        try
        {

            return Task.FromResult<IActionResult>(Ok());
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error Get log file");
            return Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {ex.Message}"));
        }
    }
}