namespace server.Configs.DbConfig;

public class MessageDatabaseConfig
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MessageCollectionName { get; set; } = null!;
}