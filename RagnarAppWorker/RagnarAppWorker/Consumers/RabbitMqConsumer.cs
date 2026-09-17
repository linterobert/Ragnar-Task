using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RagnarApp.Application.Abstract;
using RagnarApp.Application.DTOs.LibraryDTOs;
using RagnarApp.Domain.Entities;
using RagnarApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMq:UserName"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest"
        };

        _connection =
            await factory.CreateConnectionAsync();

        _channel =
            await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "books",
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer =
            new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += ConsumeMessage;

        await _channel.BasicConsumeAsync(
            queue: "books",
            autoAck: false,
            consumer: consumer);

        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }

    private async Task ConsumeMessage(
        object sender,
        BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();

        var json =
            Encoding.UTF8.GetString(body);

        var dto =
            JsonSerializer.Deserialize<ImportLibraryDTO>(json);

        if (dto is not null)
        {
            using var scope =
                _serviceProvider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var entity = new Library
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Author = dto.Author,
                Genre = MapGenre(dto.Genre),
                ImportDate = DateTimeOffset.UtcNow
            };

            await unitOfWork.LibraryRepository.Create(entity);
            await unitOfWork.Save();
        }

        await _channel!.BasicAckAsync(
            eventArgs.DeliveryTag,
            false);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();

        base.Dispose();
    }

    private static GenreEnum MapGenre(string genre)
    {
        return genre.Trim() switch
        {
            "Fantasy" => GenreEnum.Fantasy,
            "Science Fiction" => GenreEnum.ScienceFiction,
            "Dystopian" => GenreEnum.Dystopian,
            "Romance" => GenreEnum.Romance,
            "Mystery" => GenreEnum.Mystery,
            "Nonfiction" => GenreEnum.Nonfiction,
            "Classic" => GenreEnum.Classic,
            "Thriller" => GenreEnum.Thriller,
            "Technology" => GenreEnum.Technology,
            "Memoir" => GenreEnum.Memoir,

            _ => throw new ArgumentException(
                $"Unknown genre '{genre}'")
        };
    }
}
