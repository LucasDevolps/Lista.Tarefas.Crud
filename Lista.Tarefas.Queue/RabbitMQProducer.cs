using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Lista.Tarefas.Queue
{
    public class RabbitMQProducer
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly ILogger<RabbitMQProducer> _logger;

        public RabbitMQProducer(string hostname, string queueName, ILogger<RabbitMQProducer> logger)
        {
            _hostname = hostname;
            _queueName = queueName;
            _logger = logger;
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: _queueName,
                                         basicProperties: null,
                                         body: body);

                    _logger.LogInformation($"[x] Enviado: {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar enviar mensagem para o RabbitMQ: {ex.Message}");
                // Aqui você pode decidir se re-lançar a exceção ou não
            }
        }
    }
}
