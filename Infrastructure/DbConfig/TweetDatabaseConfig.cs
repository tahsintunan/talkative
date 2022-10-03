namespace server.Infrastructure.DbConfig
{
    public class TweetDatabaseConfig
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string TweetCollectionName { get; set; } = null!;
    }
}
