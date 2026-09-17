using RabbitMQ.Client;
using RagnarApp.Application.Abstract;
using System.Text;
using System.Text.Json;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;

    public RabbitMqPublisher(string hostName, string userName, string password)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    public async Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        await using var connection =
            await factory.CreateConnectionAsync();

        await using var channel =
            await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "books",
            durable: true,
            exclusive: false,
            autoDelete: false);

        var json = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "books",
            body: body);
    }
}