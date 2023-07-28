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
        Logger.Info("Start method: {0} in API Controller", nameof(GetLogFile));
        try
        {
            var stream = await _storage.GetLogFileByDate(date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller completed successfully", nameof(GetLogFile));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetLogFile));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpGet("GetLogFileToday")]
    public async Task<IActionResult> GetLogFileToday(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller", nameof(GetLogFileToday));

        try
        {
            var stream = await _storage.GetLogFileByDate(DateTime.Today.Date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller completed successfully", nameof(GetLogFileToday));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0}", nameof(GetLogFileToday));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFile")]
    public async Task<IActionResult> GetListLogFile(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller", nameof(GetListLogFile));
        try
        {
            var fileList = await _storage.GetListAvailableLogFile(cancellationToken);

            Logger.Info("Method: {0} in API Controller completed successfully", nameof(GetListLogFile));
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
        Logger.Info("Start method: {0} in API Controller", nameof(GetListLogFileByDate));
        try
        {
            if (dateFrom > dateTo)
            {
                Logger.Trace("dateFrom larger dateTo");
                return BadRequest();
            }

            var fileListByDate = await _storage.GetListAvailableLogFileByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller completed successfully", nameof(GetListLogFileByDate));
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
        Logger.Info("Start method: {0} in API Controller", nameof(GetUsers));
        try
        {
            var stream = await _storage.GetListUsers(cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller completed successfully", nameof(GetUsers));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller", nameof(GetUsers));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}