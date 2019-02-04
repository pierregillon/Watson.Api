using System;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Watson.Infrastructure.Logging
{
    public class ElasticSearchLogger : ILogger
    {
        private readonly ElasticLowLevelClient _client;

        public ElasticSearchLogger(string server, string login = null, string password = null)
        {
            var uri = new Uri(server);    
            var settings = new ConnectionConfiguration(uri);
            if (!string.IsNullOrEmpty(login) || !string.IsNullOrEmpty(password)) {
                settings.BasicAuthentication(login, password);
            }
            _client = new ElasticLowLevelClient(settings);
        }

        public async Task Log(MonitoringLogEntry logEntry)
        {
            var response = await _client.IndexAsync<StringResponse>("monitoring", logEntry.GetType().Name, logEntry.Id.ToString(), PostData.Serializable(logEntry));
            CheckReponseSuccess(response, "monitoring");
        }

        public async Task Log(ErrorLogEntry logEntry)
        {
            var response = await _client.IndexAsync<StringResponse>("error", logEntry.GetType().Name, logEntry.Id.ToString(), PostData.Serializable(logEntry));
            CheckReponseSuccess(response, "error");
        }
        
        private static void CheckReponseSuccess(StringResponse response, string index)
        {
            if (response.Success == false) {
                Console.WriteLine($"[{DateTimeOffset.UtcNow}] An error occured when logging on ElasticSearch, index '{index}'. {response.OriginalException}");
            }
        }
    }
}   