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
    public Task<Stream?> GetFile(DateTime date)
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
}