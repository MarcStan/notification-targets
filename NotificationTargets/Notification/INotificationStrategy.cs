using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets.Notification
{
    public interface INotificationStrategy
    {
        Task BroadcastNotificationAsync(MessageModel message, CancellationToken cancellationToken);
    }
}
