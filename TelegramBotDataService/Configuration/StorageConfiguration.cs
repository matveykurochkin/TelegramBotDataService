namespace TelegramBotDataService.Configuration;

public class StorageConfiguration
{
    public FileStorageConfiguration? Directory { get; init; }
}

public class FileStorageConfiguration
{
    public string? PathDirectoryToLog { get; init; }
    public string? PathDirectoryToListUsers { get; init; }
    public string? PathDirectoryToServiceLog { get; init; }
}