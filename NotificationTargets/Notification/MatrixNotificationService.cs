using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationTargets.Notification
{
    public class MatrixNotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string BaseUrl = "https://matrix.org/_matrix";
        private const string _roomId = "Notification:Matrix:RoomId", _accessToken = "Notification:Matrix:AccessToken";

        public MatrixNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsActive =>
            !string.IsNullOrEmpty(_configuration[_roomId]) &&
            !string.IsNullOrEmpty(_configuration[_accessToken]);

        public async Task SendNotificationAsync(MessageModel messageModel, CancellationToken cancellationToken)
        {
            if (!IsActive)
                return;

            // needs to be unique per message but reused for retries
            var reqId = Guid.NewGuid().ToString();

            var roomId = _configuration[_roomId];
            var accessToken = _configuration[_accessToken];
            var message = messageModel.ToString();
            var json = JsonConvert.SerializeObject(new
            {
                // https://github.com/matrix-org/matrix-doc/pull/1397/files
                msgtype = "m.text",
                // not optional. must be set to empty or message is not delivered!
                body = "",
                // allows for using html in message
                formatted_body = message,
                format = "org.matrix.custom.html"
            });
            var response = await _httpClient.PutAsync($"{BaseUrl}/client/r0/rooms/{roomId}/send/m.room.message/{reqId}?access_token={accessToken}", new StringContent(json, Encoding.UTF8, "application/jsoN"), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new WebException($"Failed to deliver notification {message} to matrix. Response: ({response.StatusCode}) {await response.Content.ReadAsStringAsync()}");
        }
    }
}
