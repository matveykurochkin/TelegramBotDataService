using NLog;

namespace TelegramBotDataService.Methods;

internal class MethodGetLogFileByDate
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    public Task<Stream?> LogFileByDate(string pathDirectory, DateTime date, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(LogFileByDate));
        var fullPath = Path.Combine(pathDirectory, $"{date:yyyy-MM-dd}.log");

        if (File.Exists(fullPath))
        {
            Logger.Info("File found {0}", fullPath);
            var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(fs as Stream)!;
        }

        Logger.Info("File not exist {0}", fullPath);
        return Task.FromResult(null as Stream);
    }
}