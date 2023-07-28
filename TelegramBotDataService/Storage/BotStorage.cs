using System.Text.RegularExpressions;
using NLog;
using TelegramBotDataService.Configuration;
using TelegramBotDataService.Methods;

namespace TelegramBotDataService.Storage;

public class BotStorage : IStorage
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly FileStorageConfiguration _configuration;

    public BotStorage(FileStorageConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Метод для получения log-файла по дате
    /// </summary>
    /// <param name="date">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Stream?> GetLogFileByDate(DateTime date, CancellationToken cancellationToken)
    {
        return new MethodGetLogFileByDate().LogFileByDate(_configuration.PathDirectoryToLog!, date, cancellationToken);
    }

    /// <summary>
    /// Метод для получения списка доступных log-файлов
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFile(CancellationToken cancellationToken)
    {
        return new MethodGetListAvailableLogFile().ListAvailableLogFile(_configuration.PathDirectoryToLog!, cancellationToken);

    }

    /// <summary>
    /// Метод возвращающий список доступных log-файлов, лежащих в промежутке указанных дат
    /// </summary>
    /// <param name="dateFrom">дата в формате yyyy-MM-dd</param>
    /// <param name="dateTo">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        return new MethodListAvailableLogFileByDate().ListAvailableLogFileByDate(_configuration.PathDirectoryToLog!, dateFrom, dateTo, cancellationToken);
    }

    /// <summary>
    /// Метод для получения списка пользователей телеграм ботом
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Stream?> GetListUsers(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetListUsers));

        var fullPath = Path.Combine(_configuration.PathDirectoryToListUsers!, "ListUsers.txt");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {0}", fullPath);
            var fileStream = new FileStream(fullPath, FileMode.Open);
            return Task.FromResult(fileStream as Stream)!;
        }

        Logger.Info("File not exist {0}", _configuration.PathDirectoryToListUsers);
        return Task.FromResult(null as Stream);
    }
}