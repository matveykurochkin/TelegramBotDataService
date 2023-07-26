using System.Text.RegularExpressions;
using NLog;
using TelegramBotDataService.Configuration;

namespace TelegramBotDataService.Storage;

public class FileStorage
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly FileStorageConfiguration _configuration;

    public FileStorage(FileStorageConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Метод для получения log файла по дате
    /// </summary>
    /// <param name="date">дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Stream?> GetLogFileByDate(DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetLogFileByDate));
        var fullPath = Path.Combine(_configuration.PathDirectoryToLog!, $"{date:yyyy-MM-dd}.log");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {0}", fullPath);
            var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(fs as Stream)!;
        }

        Logger.Info("File not exist {0}", fullPath);
        return Task.FromResult(null as Stream);
    }

    /// <summary>
    /// Метод для получения списка доступных log-файлов
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFile(CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetListAvailableLogFile));

        List<string> listLogFiles = new();

        if (Directory.Exists(_configuration.PathDirectoryToLog))
        {
            var files = Directory.GetFiles(_configuration.PathDirectoryToLog);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .ToList();

            Logger.Info("Files found {0}", _configuration.PathDirectoryToLog);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", _configuration.PathDirectoryToLog);
        return Task.FromResult(listLogFiles);
    }

    /// <summary>
    /// Метод возвращающий список доступных log-файлов, лежащих в промежутке указанных дат
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(GetListAvailableLogFileByDate));

        List<string> listLogFiles = new();

        if (Directory.Exists(_configuration.PathDirectoryToLog))
        {
            var files = Directory.GetFiles(_configuration.PathDirectoryToLog);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .Where(date =>
                {
                    if (DateTime.TryParse(date, out var currentDate))
                        return currentDate >= dateFrom && currentDate <= dateTo;
                    return false;
                })
                .ToList();

            Logger.Info("Files in the specified range are found {0}", _configuration.PathDirectoryToLog);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", _configuration.PathDirectoryToLog);
        return Task.FromResult(listLogFiles);
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