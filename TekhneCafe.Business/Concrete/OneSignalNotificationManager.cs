using Microsoft.Extensions.Configuration;
using RestSharp;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Models;

namespace TekhneCafe.Business.Concrete
{
    public class OneSignalNotificationManager : IOneSignalNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly string _appId;
        private readonly string _restApiKey;
        private readonly RestRequest _request;
        public OneSignalNotificationManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _appId = _configuration.GetValue<string>("OneSignal:AppId");
            _restApiKey = _configuration.GetValue<string>("OneSignal:RestApiKey");
            _request = new RestRequest("");
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Authorization", $"Basic {_restApiKey}");
        }

        public async Task SendToGivenUserAsync(CreateNotificationModel notificationModel, string userId)
        {
            string tagKey = "user_id";
            string tagValue = userId.ToUpper();
            var options = new RestClientOptions("https://onesignal.com/api/v1/notifications");
            var client = new RestClient(options);
            var filters = new
            {
                field = "tag",
                key = tagKey,
                relation = "=",
                value = tagValue
            };
            _request.AddJsonBody(System.Text.Json.JsonSerializer.Serialize(new
            {
                filters = new[] { filters },
                headings = new
                {
                    en = notificationModel.Title
                },
                contents = new
                {
                    en = notificationModel.Content,
                },
                app_id = _appId
            }), false);
            var response = await client.PostAsync(_request);
        }
    }
}
