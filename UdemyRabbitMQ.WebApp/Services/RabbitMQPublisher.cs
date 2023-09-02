using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace UdemyRabbitMQ.WebApp.Services
{    
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabitMQClientService = rabbitMQClientService;
        }

        public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabitMQClientService.Connect();

            var bodyString=JsonSerializer.Serialize(productImageCreatedEvent);

            var bodyByte=Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent=true;

            channel.BasicPublish
                (exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);
        }
    }
}
