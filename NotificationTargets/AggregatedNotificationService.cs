using NotificationTargets.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets
{
    /// <summary>
    /// Triggers all configured notification service.
    /// </summary>
    public class AggregatedNotificationService : INotificationStrategy
    {
        private readonly INotificationService[] _notificationServices;

        public AggregatedNotificationService(IEnumerable<INotificationService> notificationServices)
        {
            _notificationServices = notificationServices.ToArray();

            if (_notificationServices.Length == 0)
                throw new ArgumentException($"At least one {typeof(INotificationService)} must be configured for the function to run as intended but none where.");

            if (!_notificationServices.Any(x => x.IsActive))
                throw new ArgumentException("All notification targets are either disabled or misconfigured!");
        }

        public async Task BroadcastNotificationAsync(MessageModel message, CancellationToken cancellationToken)
        {
            await Task.WhenAll(_notificationServices
                  .Where(service => service.IsActive)
                  .Select(service => service.SendNotificationAsync(message, cancellationToken)));
        }
    }
}
