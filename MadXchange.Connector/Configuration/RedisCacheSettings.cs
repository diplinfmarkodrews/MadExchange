namespace MadXchange.Connector.Configuration
{
    public class RedisCacheSettings
    {
        public bool IsEnabled { get; set; }
        public string ConnectionString{ get; set; }
        public string Instance { get; set; }
    }
}