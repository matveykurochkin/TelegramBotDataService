using Microsoft.AspNetCore.Mvc;
using NLog;
using TelegramBotDataService.Storage;

namespace TelegramBotDataService.Controllers;

[Tags("Logs")]
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
    public async Task<IActionResult> GetLogFile(DateTime date, CancellationToken cancellationToken)
    {
        try
        {
            var stream = await _storage.GetLogFileByDate(date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetLogFile));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFile")]
    public async Task<IActionResult> GetListLogFile(CancellationToken cancellationToken)
    {
        try
        {
            var fileList = await _storage.GetListAvailableLogFile(cancellationToken);

            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetListLogFile));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFileByDate")]
    public async Task<IActionResult> GetListLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        try
        {
            if (dateFrom > dateTo)
            {
                Logger.Error("dateFrom larger dateTo");
                return BadRequest();
            }

            var fileListByDate = await _storage.GetListAvailableLogFileByDate(dateFrom, dateTo, cancellationToken);

            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetListLogFileByDate));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Users")]
    [HttpGet("GetListUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        try
        {
            var stream = await _storage.GetListUsers(cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetUsers));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}