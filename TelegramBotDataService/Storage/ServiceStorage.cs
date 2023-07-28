using NLog;
using TelegramBotDataService.Configuration;
using TelegramBotDataService.Methods;

namespace TelegramBotDataService.Storage;

public class ServiceStorage : IStorage
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly FileStorageConfiguration _configuration;

    public ServiceStorage(FileStorageConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Метод для получения сервисных log-файла по дате
    /// </summary>
    /// <param name="date">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Stream?> GetLogFileByDate(DateTime date, CancellationToken cancellationToken)
    {
        return new MethodGetLogFileByDate().LogFileByDate(_configuration.PathDirectoryToServiceLog!, date, cancellationToken);
    }

    /// <summary>
    /// Метод для получения списка доступных сервисных log-файлов
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFile(CancellationToken cancellationToken)
    {
        return new MethodGetListAvailableLogFile().ListAvailableLogFile(_configuration.PathDirectoryToServiceLog!, cancellationToken);
    }

    /// <summary>
    /// Метод возвращающий список доступных сервисных log-файлов, лежащих в промежутке указанных дат
    /// </summary>
    /// <param name="dateFrom">дата в формате yyyy-MM-dd</param>
    /// <param name="dateTo">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        return new MethodListAvailableLogFileByDate().ListAvailableLogFileByDate(_configuration.PathDirectoryToServiceLog!, dateFrom, dateTo, cancellationToken);
    }
}