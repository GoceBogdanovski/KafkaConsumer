using Confluent.Kafka;
using KafkaConsumer.DAL.Services;
using KafkaConsumer.Infrastructure;
using KafkaPocCommon.DTOs;
using KafkaPocCommon.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KafkaConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ConsumerService(ILogger<ConsumerService> logger, 
            IOptions<KafkaSettings> kafkaSettings,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    GroupId = _kafkaSettings.Value.GroupId,
                    BootstrapServers = _kafkaSettings.Value.BootstrapServers,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using var consumer = new ConsumerBuilder<long, string>(config)
                   .SetKeyDeserializer(new KafkaDeserializer<long>())
                   .Build();

                consumer.Subscribe(new List<string> { 
                    _kafkaSettings.Value.MaterialTopic, 
                    _kafkaSettings.Value.ShapeTopic 
                });

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    long key = consumeResult.Message.Key;

                    if(consumeResult.Message.Value is not null)
                    {
                        if (consumeResult.Topic == _kafkaSettings.Value.ShapeTopic)
                        {
                            using var scope = _serviceScopeFactory.CreateScope();
                            var _shapeDataService = scope.ServiceProvider.GetRequiredService<ShapeDataService>();
                            var message = JsonConvert.DeserializeObject<ShapeDto>(consumeResult.Message.Value);
                            await _shapeDataService.Save(key, message);

                            _logger.LogInformation($"Saved {consumeResult.Message.Value}");
                        }

                        if (consumeResult.Topic == _kafkaSettings.Value.MaterialTopic)
                        {
                            using var scope = _serviceScopeFactory.CreateScope();
                            var _materialDataService = scope.ServiceProvider.GetRequiredService<MaterialDataService>();
                            var message = JsonConvert.DeserializeObject<MaterialDto>(consumeResult.Message.Value);
                            await _materialDataService.Save(key, message);

                            _logger.LogInformation($"Saved {consumeResult.Message.Value}");
                        }
                    }                    
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError($"Consumers stoped {ex.Message} - {ex.InnerException}");
            }
        }
    }
}
