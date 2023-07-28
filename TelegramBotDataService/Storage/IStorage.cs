using System.Text.RegularExpressions;
using NLog;

namespace TelegramBotDataService.Storage;

internal interface IStorage
{
    public Task<Stream?> GetLogFileByDate(DateTime date, CancellationToken cancellationToken);
    public Task<List<string>> GetListAvailableLogFile(CancellationToken cancellationToken);
    public Task<List<string>> GetListAvailableLogFileByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken);
    
}