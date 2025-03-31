namespace Tutor.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(string topic, string message);
    }
}
