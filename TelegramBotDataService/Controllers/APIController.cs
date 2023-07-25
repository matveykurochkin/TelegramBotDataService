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
            var stream = await _storage.GetLogFileByDate(date);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error Get log file");
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFile")]
    public async Task<IActionResult> GetListLogFile()
    {
        try
        {
            var fileList = await _storage.GetListAvailableLogFile();

            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error Get list log file");
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}