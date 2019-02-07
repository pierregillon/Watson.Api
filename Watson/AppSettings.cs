namespace Watson.Server
{
    public class AppSettings
    {
        public ServerConfiguration ElasticSearch { get; set; } = new ServerConfiguration();
        public ServerConfiguration EventStore { get; set; } = new ServerConfiguration();
    }

    public class ServerConfiguration
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
