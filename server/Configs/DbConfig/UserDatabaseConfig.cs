namespace server.Configs.DbConfig
{
    public class UserDatabaseConfig
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserCollectionName { get; set; } = null!;

        public string RefreshTokenCollectionName { get; set; } = null!;
    }
}
