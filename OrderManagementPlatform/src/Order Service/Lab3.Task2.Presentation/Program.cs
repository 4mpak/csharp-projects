using Lab3.Task1.Application.Extension;
using Lab3.Task1.Infrastructure.Extension;
using Lab3.Task2.Presentation.BackgroundServices;
using Lab3.Task2.Presentation.Extensions;
using Lab3.Task2.Presentation.Interceptor;
using Lab3.Task2.Presentation.Services;
using Lab4.Task1.Kafka.Extensions;
using Orders.Kafka.Contracts;
using Task1.DependencyInjection;
using Task1.Options;
using Task2.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.SettingBuilder(builder.Configuration);

builder.Services.AddSingleton(builder.Configuration);
builder.Services.ConfigureSettingsOptions();
builder.Services.AddManualClient();
builder.Services.AddService();

builder.Services.AddHostedService<MyBackgroundService>();

builder.Services.AddMyConnections(builder.Configuration);
builder.Services.AddMigrations();
builder.Services.AddRepositories();
builder.Services.AddKafkaExtension();
builder.Services.AddServices();
builder.Services.AddKafkaConsumer<OrderProcessingKey, OrderProcessingValue>();
builder.Services.AddConsumer();
builder.Services.AddKafkaProducer<OrderCreationKey, OrderCreationValue>();
builder.Services.AddGrpc();
builder.Services.AddHostedService<KafkaBackgroundService<OrderProcessingKey, OrderProcessingValue>>();

builder.Services.AddGrpcReflection();
builder.Services.AddGrpc(grpc => grpc.Interceptors.Add<MyServerInterceptor>());

WebApplication app = builder.Build();

app.MapGrpcService<OrderController>();
app.MapGrpcService<ProductController>();
app.MapGrpcReflectionService();
app.Run();
