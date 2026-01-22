using FluentMigrator.Runner;
using Lab3.Task1.Application.Constans.DTO;
using Lab3.Task1.Application.Constans.Services;
using Lab3.Task1.Application.Extension;
using Lab3.Task1.Infrastructure.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task1.DependencyInjection;
using Task1.Options;
using Task2.DependencyInjection;
using Task2.Interfaces;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

var serviceCollection = new ServiceCollection();
var builder = new ConfigurationBuilder();
builder.AddJsonFile(@"D:\прога\RiderProjects\CSharpMicroservices\src\lab-2\Settings.json");
serviceCollection.SettingBuilder(builder);
IConfiguration configuration = builder.Build();
serviceCollection.AddSingleton(configuration);

serviceCollection.ConfigureSettingsOptions();
serviceCollection.AddManualClient();
serviceCollection.AddService();
ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

IConfigurationsService configService = serviceProvider.GetRequiredService<IConfigurationsService>();
var cancellationTokenSource = new CancellationTokenSource();
CancellationToken token = cancellationTokenSource.Token;
await configService.FetchAndUpdateConfigurations(token);

_ = configService.Run(token);

serviceCollection.AddMyConnections(configuration);
serviceCollection.AddMigrations();
serviceCollection.AddRepositories();
serviceCollection.AddServices();
serviceProvider = serviceCollection.BuildServiceProvider();

IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

IOrderService orderService = serviceProvider.GetRequiredService<IOrderService>();
IProductService productService = serviceProvider.GetRequiredService<IProductService>();

var dto1 = new ProductCreatingDto { Name = "sosiska", Price = 52 };
long product1 = await productService.CreateProductAsync(dto1);

var dto2 = new ProductCreatingDto { Name = "pureshka", Price = 228 };
long product2 = await productService.CreateProductAsync(dto2);

var dto3 = new ProductCreatingDto { Name = "bon pari koshmariki", Price = 666 };
long product3 = await productService.CreateProductAsync(dto3);

var orderDto = new OrderCreatingDto { OrderCreatedBy = "Walter White" };
long order = await orderService.CreateOrderAsync(orderDto);

await orderService.AddProductAsync(order, product1, 7);
await orderService.AddProductAsync(order, product2, 1);
await orderService.AddProductAsync(order, product3, 3);

await orderService.DeleteProductAsync(order, product2);

await orderService.ChangeStateToProcessingAsync(order);
await orderService.ChangeStateToCompletedAsync(order);

IAsyncEnumerable<OrderHistoryDto> walterOrderHistory = orderService.QueryOrderHistory(order, null, 5, 0);
await foreach (OrderHistoryDto orderHistoryDto in walterOrderHistory)
{
    Console.WriteLine($"OrderId: {orderHistoryDto.OrderHistoryItemId}");
}