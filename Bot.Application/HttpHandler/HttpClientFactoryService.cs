using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using Bot.Application.Bot.Commands;
using Bot.Application.Contracts;

namespace Bot.Application.HttpHandler
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private static AppSettings _appSettings;
        private static string _watermark = null;
        private readonly IDistributedCache _distributedCache;
        public HttpClientFactoryService(IHttpClientFactory httpClientFactory, AppSettings appSettings,
            IDistributedCache distributedCache)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _distributedCache = distributedCache;
        }

        public async Task<DirectLineToken> GetTokenAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            using (var response = await httpClient.GetAsync(_appSettings.BotTokenEndpoint, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                var token = await JsonSerializer.DeserializeAsync<DirectLineToken>(stream, _options);

                return token;
            }
        }

        public async Task<RegionalChannelSettingsDirectLine> GetRegionalChannelSettingsDirectline(string tokenEndpoint)
        {
            string environmentEndPoint = tokenEndpoint.Substring(0, tokenEndpoint.IndexOf("/powervirtualagents/"));
            string apiVersion = tokenEndpoint.Substring(tokenEndpoint.IndexOf("api-version")).Split("=")[1];
            var regionalChannelSettingsURL = $"{environmentEndPoint}/powervirtualagents/regionalchannelsettings?api-version={apiVersion}";

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                using (var response = await httpClient.GetAsync(regionalChannelSettingsURL, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync();

                    var regionalSettings = await JsonSerializer.DeserializeAsync<RegionalChannelSettingsDirectLine>(stream, _options);

                    return regionalSettings;
                }

            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }

        }

        public async Task<StartConversationResponse> StartConversation(string token)
        {
            var httpClient = _httpClientFactory.CreateClient();

            //---- Create headers
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            using (var response = await httpClient.PostAsync(_appSettings.BotconversationsEndpoint, null))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var conversationId = await JsonSerializer.DeserializeAsync<StartConversationResponse>(stream, _options);

                return conversationId;
            }
        }

        public async Task<dynamic> Conversation(string conversationtId, string token, Request request)
        {
            var data = JsonSerializer.Serialize(request);

            var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();

            //---- Create headers
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var url = $"{_appSettings.BotconversationActivitiesEndpoint}{conversationtId}/activities";

            using (var response = await httpClient.PostAsync(url, jsonContent))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                var results = await JsonSerializer.DeserializeAsync<ActivityResponse>(stream, _options);

                if (results != null)
                {
                    return results.id;
                }

                return 0;
            }
        }

        public async Task<List<Activity>> GetBotResponseActivitiesAsync(string conversationtId, string token)
        {
            ActivitySet response = null;
            List<Activity> result = new List<Activity>();

           
                var httpClient = _httpClientFactory.CreateClient();

                var url = $"{_appSettings.BotconversationActivitiesEndpoint}{conversationtId}/activities";
                //---- Create headers
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                using (var _response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    var stream = await _response.Content.ReadAsStreamAsync();
                string text = string.Empty;
                string results = string.Empty;
                using (StreamReader reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                    // Now 'text' contains the content of the stream as a string
                }
                var dresults  = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivitySet>(text);
                if (dresults == null)
                    {
                        // response can be null if directLineClient token expires
                        Console.WriteLine("Conversation expired. Press any key to exit.");
                        Console.Read();
                        //directLineClient.Dispose();
                        //Environment.Exit(0);
                    }

                    _watermark = dresults?.Watermark;
                    result = dresults?.Activities?.Where(x =>
                      x.Type == ActivityTypes.Message &&
                        string.Equals(x.From.Name, _appSettings.BotName, StringComparison.Ordinal)).ToList();

                    if (result != null && result.Any())
                    {
                        return result;
                    }
                }

          //  response != null && response.Activities.Any();

            return new List<Activity>();
        }

        public async Task<DirectLineToken> GetTokenFromCacheAsync(string deviceId, CancellationToken cancellationToken)
        {
            var tokenResponse = new DirectLineToken();
            if (!string.IsNullOrEmpty(deviceId))
            {
                //check if token is cached
                try
                {
                    tokenResponse = await GetTokenAsync();
                }
                catch (Exception ex)
                {
                    tokenResponse = await GetTokenAsync();
                    Serilog.Log.Error("HttpClientFactoryService :GetTokenFromCacheAsync =>", ex.Message);
                }
            }
            return tokenResponse;
        }
    }
}
