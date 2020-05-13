using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets.Notification
{
    /// <summary>
    /// Helper that posts a message to slack.
    /// </summary>
    public class SlackNotificationService : INotificationService
    {
        private const string _webHookKey = "Notification:Slack:Webhook";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SlackNotificationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public bool IsActive => !string.IsNullOrEmpty(_configuration[_webHookKey]);

        public Task SendNotificationAsync(MessageModel message, CancellationToken cancellationToken)
        {
            if (!IsActive)
                return Task.CompletedTask;

            var webHookUrl = _configuration[_webHookKey];

            var json = JsonConvert.SerializeObject(new { text = $"{message.Title}: {message.Message}" });
            return _httpClient.PostAsync(webHookUrl, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
        }
    }
}