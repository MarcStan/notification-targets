using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets.Notification
{
    /// <summary>
    /// Helper that sends a message via email using sendgrid.
    /// </summary>
    public class SendGridNotificationService : INotificationService
    {
        private const string _apiKeyKey = "Notification:SendGrid:Key", _fromKey = "Notification:SendGrid:From", _toKey = "Notification:SendGrid:To";

        private readonly IConfiguration _configuration;

        public SendGridNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsActive =>
            !string.IsNullOrEmpty(_configuration[_apiKeyKey]) &&
            !string.IsNullOrEmpty(_configuration[_fromKey]) &&
            !string.IsNullOrEmpty(_configuration[_toKey]);

        public async Task SendNotificationAsync(MessageModel message, CancellationToken cancellationToken)
        {
            if (!IsActive)
                return;

            var key = _configuration[_apiKeyKey];
            var from = _configuration[_fromKey];

            var client = new SendGridClient(key);
            var addresses = (_configuration[_toKey] ?? throw new KeyNotFoundException($"Missing key '{_fromKey}'")).Split(',', ';');
            await Task.WhenAll(addresses.Select(to => client.SendEmailAsync(MailHelper.CreateSingleEmail(new EmailAddress(from), new EmailAddress(to), message.Title, message.Message, null), cancellationToken)));
        }
    }
}
