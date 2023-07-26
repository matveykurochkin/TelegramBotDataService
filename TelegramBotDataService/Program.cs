using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog.Web;
using TelegramBotDataService.Configuration;
using TelegramBotDataService.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    var kestrelSection = context.Configuration.GetSection("Http");
    serverOptions.Configure(kestrelSection);
});

builder.Host.ConfigureServices((hostBuilderContext, serviceCollection) =>
{
    serviceCollection.AddOptions<StorageConfiguration>()
        .Bind(hostBuilderContext.Configuration.GetSection("Storage"));

    serviceCollection.AddSingleton<FileStorage>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<StorageConfiguration>>();
        var storageConfiguration = options.Value;

        var fileStorage = new FileStorage(storageConfiguration.Directory!);

        return fileStorage;
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(settings =>
{
    settings.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Schedule Bot Data Service", 
        Version = "v1.1", 
        Description = "Schedule Bot Data Service is a RESTful API service that provides interaction with a telegram bot and provides an opportunity to receive log files created by the bot during its operation.", 
        Contact = new OpenApiContact
        {
            Name = "Matvey Kurochkin",
            Url = new Uri("https://github.com/matveykurochkin/TelegramBotDataService")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(settings =>
    {
        settings.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule Bot Data Service API V1.1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();