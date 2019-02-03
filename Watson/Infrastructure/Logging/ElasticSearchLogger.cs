using System;
using System.Threading.Tasks;

namespace Watson.Infrastructure.Logging
{
    public class ElasticSearchLogger : ILogger
    {
        // private readonly ElasticClient _elasticClient;

        // public ElasticSearchLogger(string elasticSearchUrl, string login, string password)
        // {
        //     var uri = new Uri(elasticSearchUrl);
        //     var pool = new SingleNodeConnectionPool(uri);
        //     var settings = new ConnectionSettings(pool);

        //     if (!string.IsNullOrEmpty(login) || !string.IsNullOrEmpty(password)) {
        //         settings.BasicAuthentication(login, password);
        //     }
        //     _elasticClient = new ElasticClient(settings);
        // }

        public Task Log(MonitoringLogEntry data)
        {
            return Task.Delay(0);
            // return _elasticClient.IndexAsync(data, idx => idx.Index("monitoring"));
        }
        public Task Log(ErrorLogEntry data)
        {
            return Task.Delay(0);
            // return _elasticClient.IndexAsync(data, idx => idx.Index("errors"));
        }
    }
}   