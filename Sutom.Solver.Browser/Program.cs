var hostBuilder = Host.CreateDefaultBuilder(args);
var host = hostBuilder
    .ConfigureServices((_, services) =>
    {
        services.AddOptions();
        services.AddSingleton<GameSolverApplication>();
        services.AddSingleton<IWebDriverFactory, EdgeDriverFactory>();
    })
    .UseConsoleLifetime()
    .Build();

using var serviceScope = host.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var application = services.GetRequiredService<GameSolverApplication>();

var configuration = new ApplicationConfiguration();
new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().Bind(configuration);
await application.Run(configuration);