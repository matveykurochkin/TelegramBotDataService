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

    /// <summary>
    /// Группа методов, использующихся для просмотра списка пользователей ботом и
    /// log-файлов, созданных ботом
    /// </summary>
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

    /// <summary>
    /// Группа методов, использующихся для просмтора log-файлов, созданных сервисом
    /// </summary>
    [Tags("Service Logs")]
    [HttpGet("GetServiceLogFileByDate")]
    public async Task<IActionResult> GetServiceByDate(DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetServiceByDate), nameof(APIController));

        try
        {
            var stream = await _serviceLogStorage.GetByDate(date, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetServiceByDate), nameof(APIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetServiceByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [Tags("Service Logs")]
    [HttpPost("GetServiceListAvailableLogFileByDate")]
    public async Task<IActionResult> GetServiceListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetServiceListAvailableByDate), nameof(APIController));
        try
        {
            var fileListByDate = await _serviceLogStorage.GetListAvailableByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetServiceListAvailableByDate), nameof(APIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetServiceListAvailableByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}