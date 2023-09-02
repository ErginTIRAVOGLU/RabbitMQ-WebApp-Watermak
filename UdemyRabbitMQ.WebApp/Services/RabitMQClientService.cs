using RabbitMQ.Client;

namespace UdemyRabbitMQ.WebApp.Services
{
    public class RabitMQClientService:IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabitMQClientService> _logger;

        public RabitMQClientService(ConnectionFactory connectionFactory, ILogger<RabitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            Connect();
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            
            if(_channel is { IsOpen:true } )
            {
                return _channel;
            }

            _channel=_connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);

            _channel.QueueDeclare(QueueName, true, false, false, null);

            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

            _logger.LogInformation("RabbbitMQ ile bağlantı kuruldu.");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            
            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbbitMQ ile bağlantı kapandı.");

        }
    }
}
