namespace MadXchange.Connector.Configuration
{
    public class RedisCacheSettings
    {
        public bool enabled { get; set; }
        public string ConnectionString{ get; set; }
        public string Instance { get; set; }
    }
}