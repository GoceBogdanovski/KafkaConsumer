using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace KafkaConsumer.Infrastructure
{
    public class KafkaDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default;
            }

            try
            {
                var json = Encoding.UTF8.GetString(data.ToArray());

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
