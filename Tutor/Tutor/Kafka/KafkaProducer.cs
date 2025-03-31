using Confluent.Kafka;

namespace Tutor.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _config;

        public KafkaProducer(string bootstrapServers)
        {
            _config = new ProducerConfig { BootstrapServers = bootstrapServers };
        }

        public async Task ProduceAsync(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            }
        }
    }
}
