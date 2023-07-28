using Microsoft.AspNetCore.Mvc;
using NLog;
using TelegramBotDataService.Storage;

namespace TelegramBotDataService.Controllers;

[Tags("Schedule Bot Logs")]
[Route("api")]
[ApiController]
// ReSharper disable once InconsistentNaming
public class BotAPIController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly BotStorage _storage;

    public BotAPIController(BotStorage storage)
    {
        _storage = storage;
    }

    [HttpGet("GetLogFileByDate")]
    public async Task<IActionResult> GetLogFileByDate(DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetLogFileByDate), nameof(BotAPIController));
        try
        {
            var stream = await _storage.GetLogFileByDate(date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetLogFileByDate), nameof(BotAPIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetLogFileByDate), nameof(BotAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpGet("GetLogFileToday")]
    public async Task<IActionResult> GetLogFileToday(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetLogFileToday), nameof(BotAPIController));

        try
        {
            var stream = await _storage.GetLogFileByDate(DateTime.Today.Date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetLogFileToday), nameof(BotAPIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetLogFileToday), nameof(BotAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFile")]
    public async Task<IActionResult> GetListAvailableLogFile(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableLogFile), nameof(BotAPIController));
        try
        {
            var fileList = await _storage.GetListAvailableLogFile(cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableLogFile), nameof(BotAPIController));
            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableLogFile), nameof(BotAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFileByDate")]
    public async Task<IActionResult> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableLogFileByDate), nameof(BotAPIController));
        try
        {
            if (dateFrom > dateTo)
            {
                Logger.Trace("dateFrom larger dateTo");
                return BadRequest();
            }

            var fileListByDate = await _storage.GetListAvailableLogFileByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableLogFileByDate), nameof(BotAPIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableLogFileByDate), nameof(BotAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Schedule Bot Users")]
    [HttpGet("GetListUsers")]
    public async Task<IActionResult> GetListUsers(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListUsers), nameof(BotAPIController));
        try
        {
            var stream = await _storage.GetListUsers(cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListUsers), nameof(BotAPIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListUsers), nameof(BotAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}