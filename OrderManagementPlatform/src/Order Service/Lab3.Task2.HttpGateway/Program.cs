using Lab3.Task2.HttpGateway.Extensions;
using Lab3.Task2.HttpGateway.Mappers;
using Lab3.Task2.HttpGateway.Middleware;
using Lab3.Task2.HttpGateway.Services;
using Lab3.Task2.HttpGateway.Services.Interfaces;
using Lab3.Task2.Presentation.BackgroundServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task1.DependencyInjection;
using Task1.Options;
using Task2.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ClientSetting.json");
builder.Services.SettingBuilder(builder.Configuration);

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton<OrderMapper>();
builder.Services.AddScoped<IProductGatewayService, ProductGatewayService>();
builder.Services.AddScoped<IOrderGatewayService, OrderGatewayService>();
builder.Services.ConfigureSettingsOptions();
builder.Services.AddManualClient();
builder.Services.AddService();

builder.Services.AddHostedService<MyBackgroundService>();
builder.Services.AddClients();
builder.Services.AddControllers();
builder.Services.AddConnections();
builder.Services.AddScoped<MyMiddleware>();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();
app.UseMiddleware<MyMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();