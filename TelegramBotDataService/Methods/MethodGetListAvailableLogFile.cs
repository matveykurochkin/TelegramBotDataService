using System.Text.RegularExpressions;
using NLog;

namespace TelegramBotDataService.Methods;

internal class MethodGetListAvailableLogFile
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    public Task<List<string>> ListAvailableLogFile(string pathDirectory,CancellationToken cancellationToken)
    {
        Logger.Info("Start method: {0}", nameof(ListAvailableLogFile));

        List<string> listLogFiles = new();

        if (Directory.Exists(pathDirectory))
        {
            var files = Directory.GetFiles(pathDirectory);

            listLogFiles = files
                .Select(file => Regex.Match(file, @"\d{4}-\d{2}-\d{2}").Value)
                .ToList();

            Logger.Info("Files found {0}", pathDirectory);
            return Task.FromResult(listLogFiles);
        }

        Logger.Info("Files not exist {0}", pathDirectory);
        return Task.FromResult(listLogFiles);
    }
    
}