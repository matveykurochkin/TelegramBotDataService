using TelegramBotDataService.Agent;
using TelegramBotDataService.Configuration;

namespace TelegramBotDataService.Storage;

public class ServiceLogStorage : ILogStorage
{
    private readonly PathConfiguration _configuration;

    public ServiceLogStorage(PathConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<Stream?> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        return new LogDirectoryAgent().LogFileByDate(_configuration.PathDirectoryToServiceLog!, cancellationToken, date);
    }

    public Task<List<string>> GetListAvailableByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        return new LogDirectoryAgent().ListAvailableLogFileByDate(_configuration.PathDirectoryToServiceLog!, cancellationToken, dateFrom, dateTo);
    }
}