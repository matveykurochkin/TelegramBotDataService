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

    private readonly BotDataStorage _botDataStorage;

    public APIController(BotDataStorage botDataStorage)
    {
        _botDataStorage = botDataStorage;
    }

    /// <summary>
    /// Группа методов, использующихся для просмотра списка пользователей ботом и
    /// log-файлов, созданных ботом
    /// </summary>
    [Tags("Schedule Bot Logs")]
    [HttpGet("GetLogFileByDate")]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetByDate), nameof(APIController));
        try
        {
            var stream = await _botDataStorage.GetByDate(date, cancellationToken);

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
    
    [Tags("Schedule Bot Logs")]
    [HttpPost("GetListAvailableLogFileByDate")]
    public async Task<IActionResult> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
        try
        {
            var fileListByDate = await _botDataStorage.GetListAvailableByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableByDate), nameof(APIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableByDate), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
    
    [Tags("Schedule Bot Users from File Storage")]
    [HttpGet("GetListUsers")]
    public async Task<IActionResult> GetListUsers(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListUsers), nameof(APIController));
        try
        {
            var stream = await _botDataStorage.GetListUsers(cancellationToken);

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

    [Tags("Schedule Bot Users from Data Base")]
    [HttpGet("GetListUsersFromDB")]
    public async Task<IActionResult> GetListUsersFromDb(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _botDataStorage.GetListUsersFromDb(cancellationToken);

            if (data == null)
            {
                Logger.Info("Users not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListUsersFromDb), nameof(APIController));
            return Ok(data);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListUsersFromDb), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
    
    [Tags("Schedule Bot Users from Data Base")]
    [HttpGet("GetCountMessageFromDb")]
    public async Task<IActionResult> GetCountMessageFromDb(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _botDataStorage.GetCountMessageFromDb(cancellationToken);

            if (data.CountOfMessages == 0)
            {
                Logger.Info("Count messages not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetCountMessageFromDb), nameof(APIController));
            return Ok(data);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetCountMessageFromDb), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
    
    [Tags("Schedule Bot Users from Data Base")]
    [HttpGet("GetLastUserFromDb")]
    public async Task<IActionResult> GetLastUserFromDb(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _botDataStorage.GetLastUserFromDb(cancellationToken);

            if (data == null)
            {
                Logger.Info("Last user not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetLastUserFromDb), nameof(APIController));
            return Ok(data);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetLastUserFromDb), nameof(APIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}