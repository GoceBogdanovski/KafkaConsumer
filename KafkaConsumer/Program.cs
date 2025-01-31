using KafkaConsumer.Services;
using KafkaConsumer.DAL;
using Microsoft.EntityFrameworkCore;
using KafkaPocCommon.Infrastructure;
using KafkaConsumer.DAL.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddHostedService<ConsumerService>();
builder.Services.AddScoped<ShapeDataService>();
builder.Services.AddScoped<MaterialDataService>();

builder.Services.AddDbContext<EntityDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

var app = builder.Build();
app.Run();
