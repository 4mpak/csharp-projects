using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task3.DependencyInjection;
using Task3.Service;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile(@"D:\прога\RiderProjects\CSharpMicroservices\Settings.json", optional: true, reloadOnChange: true)
    .Build();
var services = new ServiceCollection();
services.AddHttpClient();
services.AddSingleton<IConfiguration>(configuration);
services.AddDisplayService();
ServiceProvider serviceProvider = services.BuildServiceProvider();
IConsoleService consoleService = serviceProvider.GetRequiredService<IConsoleService>();
using var cts = new CancellationTokenSource();
await consoleService.Run(cts.Token);