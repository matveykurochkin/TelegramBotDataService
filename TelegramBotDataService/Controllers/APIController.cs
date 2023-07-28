using Microsoft.AspNetCore.Mvc;
using NLog;
using TelegramBotDataService.Storage;

namespace TelegramBotDataService.Controllers;

[Tags("Schedule Bot Logs")]
[Route("api")]
[ApiController]
// ReSharper disable once InconsistentNaming
public class APIController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly BotLogStorage _botLogStorage;
    private readonly ServiceLogStorage _serviceLogStorage;

    public APIController(BotLogStorage botLogStorage, ServiceLogStorage serviceLogStorage)
    {
        _botLogStorage = botLogStorage;
        _serviceLogStorage = serviceLogStorage;
    }

    [HttpGet("GetLogFileByDate")]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetByDate), nameof(APIController));
        try
        {
            var stream = await _botLogStorage.GetByDate(date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetByDate), nameof(APIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFile")]
    public async Task<IActionResult> GetListAvailable(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailable), nameof(APIController));
        try
        {
            var fileList = await _botLogStorage.GetListAvailable(cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailable), nameof(APIController));
            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailable), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetListAvailableLogFileByDate")]
    public async Task<IActionResult> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
        try
        {
            var fileListByDate = await _botLogStorage.GetListAvailableByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableByDate), nameof(APIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Schedule Bot Users")]
    [HttpGet("GetListUsers")]
    public async Task<IActionResult> GetListUsers(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListUsers), nameof(APIController));
        try
        {
            var stream = await _botLogStorage.GetListUsers(cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListUsers), nameof(APIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListUsers), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Service Logs")]
    [HttpGet("GetServiceLogFileByDate")]
    public async Task<IActionResult> GetServiceByDate(DateTime dateTime, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetByDate), nameof(APIController));

        try
        {
            var stream = await _serviceLogStorage.GetByDate(dateTime, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetByDate), nameof(APIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Service Logs")]
    [HttpPost("GetServiceListAvailableLogFile")]
    public async Task<IActionResult> GetServiceListAvailable(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailable), nameof(APIController));
        try
        {
            var fileList = await _serviceLogStorage.GetListAvailable(cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailable), nameof(APIController));
            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailable), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Service Logs")]
    [HttpPost("GetServiceListAvailableLogFileByDate")]
    public async Task<IActionResult> GetServiceListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
        try
        {
            if (dateFrom > dateTo)
            {
                Logger.Trace("dateFrom larger dateTo");
                return BadRequest();
            }

            var fileListByDate = await _serviceLogStorage.GetListAvailableByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableByDate), nameof(APIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}