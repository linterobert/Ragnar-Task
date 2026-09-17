namespace RagnarApp.Application.Abstract
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(
            T message,
            CancellationToken cancellationToken = default);
    }
}
