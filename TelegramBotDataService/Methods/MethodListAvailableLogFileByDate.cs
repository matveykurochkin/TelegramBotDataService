using System.Text.RegularExpressions;
using NLog;

namespace TelegramBotDataService.Methods;

internal class MethodListAvailableLogFileByDate
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
    public Task<List<string>> ListAvailableLogFileByDate(string pathDirectory, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(ListAvailableLogFileByDate));

        List<string> listLogFiles = new();

        if (Directory.Exists(pathDirectory))
        {
            var files = Directory.GetFiles(pathDirectory);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .Where(date =>
                {
                    if (DateTime.TryParse(date, out var currentDate))
                        return currentDate >= dateFrom && currentDate <= dateTo;
                    return false;
                })
                .ToList();

            Logger.Info("Files in the specified range are found {0}", pathDirectory);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", pathDirectory);
        return Task.FromResult(listLogFiles);
    }
}