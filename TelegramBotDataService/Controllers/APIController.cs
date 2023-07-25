using Microsoft.AspNetCore.Mvc;
using NLog;
using TelegramBotDataService.Storage;

namespace TelegramBotDataService.Controllers;

[Route("api")]
[ApiController]
// ReSharper disable once InconsistentNaming
public class APIController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly FileStorage _storage;

    public APIController(FileStorage storage)
    {
        _storage = storage;
    }

    [HttpGet("GetLogFileByDate")]
    public async Task<IActionResult> GetLogFile(DateTime date)
    {
        try
        {
            var stream = await _storage.GetFile(date);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }
            
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error Get log file");
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {ex.Message}"));
        }
    }
}