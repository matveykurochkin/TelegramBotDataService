using Microsoft.AspNetCore.Mvc;
using NLog;
using TelegramBotDataService.Storage;

namespace TelegramBotDataService.Controllers;

[Tags("Service Logs")]
[Route("serviceApi")]
[ApiController]
// ReSharper disable once InconsistentNaming
public class ServiceAPIController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ServiceStorage _storage;

    public ServiceAPIController(ServiceStorage storage)
    {
        _storage = storage;
    }

    [HttpGet("GetServiceLogFileByDate")]
    public async Task<IActionResult> GetLogFileByDate(DateTime dateTime, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetLogFileByDate), nameof(ServiceAPIController));

        try
        {
            var stream = await _storage.GetLogFileByDate(dateTime, cancellationToken);

            if (stream == null)
            {
                Logger.Info("File not found");
                return NotFound();
            }

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetLogFileByDate), nameof(ServiceAPIController));
            return new FileStreamResult(stream, "text/plain");
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetLogFileByDate), nameof(ServiceAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetServiceListAvailableLogFile")]
    public async Task<IActionResult> GetListAvailableLogFile(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableLogFile), nameof(ServiceAPIController));
        try
        {
            var fileList = await _storage.GetListAvailableLogFile(cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableLogFile), nameof(ServiceAPIController));
            return new JsonResult(fileList);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableLogFile), nameof(ServiceAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }

    [HttpPost("GetServiceListAvailableLogFileByDate")]
    public async Task<IActionResult> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0} in API Controller: {1}", nameof(GetListAvailableLogFileByDate), nameof(ServiceAPIController));
        try
        {
            if (dateFrom > dateTo)
            {
                Logger.Trace("dateFrom larger dateTo");
                return BadRequest();
            }

            var fileListByDate = await _storage.GetListAvailableLogFileByDate(dateFrom, dateTo, cancellationToken);

            Logger.Info("Method: {0} in API Controller: {1} completed successfully", nameof(GetListAvailableLogFileByDate), nameof(ServiceAPIController));
            return new JsonResult(fileListByDate);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Error in method: {0} in API Controller: {1}", nameof(GetListAvailableLogFileByDate), nameof(ServiceAPIController));
            return await Task.FromResult<IActionResult>(StatusCode(500, $"An error occurred: {exception.Message}"));
        }
    }
}