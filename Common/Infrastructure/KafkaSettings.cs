namespace KafkaPocCommon.Infrastructure
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public string ShapeTopic { get; set; }
        public string MaterialTopic { get; set; }
    }
}
