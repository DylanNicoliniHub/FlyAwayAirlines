using Infrastructure.Messaging;

namespace Infrastructure.Azure
{
    /// <summary>
    /// If a command message implements <see cref="IMessageSessionProvider"/>, it hints implementations of 
    /// <see cref="ICommandBus"/> to assign the specified SessionId to the outgoing messages if supported.
    /// </summary>
    public interface IMessageSessionProvider
    {
        string SessionId { get; }
    }
}