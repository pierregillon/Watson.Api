using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Watson.Infrastructure.Logging
{
    public class ElasticSearchLogger : ILogger
    {
        private readonly ElasticLowLevelClient _client;

        public ElasticSearchLogger(string server, int port = 9200, string login = null, string password = null)
        {
            var uri = new Uri($"http://{server}:{port}");    
            var settings = new ConnectionConfiguration(uri);
            if (!string.IsNullOrEmpty(login) || !string.IsNullOrEmpty(password)) {
                settings.BasicAuthentication(login, password);
            }
            _client = new ElasticLowLevelClient(settings);
        }

        public Task Log(MonitoringLogEntry logEntry)
        {
            ThreadPool.QueueUserWorkItem(async x => {
                var response = await _client.IndexAsync<StringResponse>("monitoring", logEntry.GetType().Name, logEntry.Id.ToString(), PostData.Serializable(logEntry));
                CheckReponseSuccess(response, "monitoring");
            });

            return Task.Delay(0);
        }

        public Task Log(ErrorLogEntry logEntry)
        {
            ThreadPool.QueueUserWorkItem(async x => {
                var response = await _client.IndexAsync<StringResponse>("error", logEntry.GetType().Name, logEntry.Id.ToString(), PostData.Serializable(logEntry));
                CheckReponseSuccess(response, "error");
            });
            
            return Task.Delay(0);
        }
        
        private static void CheckReponseSuccess(StringResponse response, string index)
        {
            if (response.Success == false) {
                Console.WriteLine($"[{DateTimeOffset.UtcNow}] An error occured when logging on ElasticSearch, index '{index}'. {response.OriginalException.Message}");
            }
        }
    }
}   