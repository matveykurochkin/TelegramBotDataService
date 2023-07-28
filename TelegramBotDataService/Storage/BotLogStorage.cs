using NLog;
using TelegramBotDataService.Agent;
using TelegramBotDataService.Configuration;

namespace TelegramBotDataService.Storage;

public class BotLogStorage
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly PathConfiguration _configuration;

    public BotLogStorage(PathConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<Stream?> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        return new LogDirectoryAgent().LogFileByDate(_configuration.PathDirectoryToLog!, cancellationToken, date);
    }

    public Task<List<string>> GetListAvailable(CancellationToken cancellationToken)
    {
        return new LogDirectoryAgent().ListAvailableLogFile(_configuration.PathDirectoryToLog!, cancellationToken);
    }

    public Task<List<string>> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        return new LogDirectoryAgent().ListAvailableLogFileByDate(_configuration.PathDirectoryToLog!, cancellationToken, dateFrom, dateTo);
    }

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