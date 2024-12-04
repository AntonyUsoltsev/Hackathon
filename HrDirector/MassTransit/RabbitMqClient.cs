using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace HrDirector.MassTransit;

public class RabbitMqClient : IDisposable
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly IConnection _connection;
    // private readonly IModel _channel;

    public RabbitMqClient(string hostname, string queueName)
    {
        _hostname = hostname;
        _queueName = queueName;
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = factory.CreateConnection();
        // _channel = _connection.CreateModel();
    }

    public void SendMessage<T>(T message)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        string jsonMessage = JsonSerializer.Serialize(message);
        byte[] body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        Console.WriteLine($"Message sent to queue {_queueName}: {jsonMessage}");
    }
    
    public void Dispose()
    {
        // _channel.Close();
        _connection.Close();
    }
}