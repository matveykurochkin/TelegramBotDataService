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
    /// <returns></returns>
    public Task<Stream?> GetLogFileByDate(DateTime date)
    {
        Logger.Info("Start get log file");
        var fullPath = Path.Combine(_configuration.PathDirectory!, $"{date:yyyy-MM-dd}.log");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {fullPath}", fullPath);
            var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(fs as Stream)!;
        }

        Logger.Info("File not exist {fullPath}", fullPath);
        return Task.FromResult(null as Stream);
    }

    /// <summary>
    /// Метод для получения списка доступных log-файлов
    /// </summary>
    /// <returns></returns>
    public Task<List<string>> GetListAvailableLogFile()
    {
        Logger.Info("Start Get List Available Log File");

        List<string> listLogFiles = new();

        if (Directory.Exists(_configuration.PathDirectory))
        {
            var files = Directory.GetFiles(_configuration.PathDirectory);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .ToList();

            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {fullPath}", _configuration.PathDirectory);
        return Task.FromResult(listLogFiles);
    }
}