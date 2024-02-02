using Elephanel;
using Npgsql;

namespace Elephanel;

public class PostgresConnection : IDisposable
{
    private bool isOpened = false;

    public static PostgresConnection Create() => Create(Program.botConfig.PostgresConnectionString);
    public static PostgresConnection Create(string connectionString)
    {
        return new PostgresConnection(connectionString);
    }

    public NpgsqlConnection connection;
    private PostgresConnection(string connectionString)
    {
        connection = new NpgsqlConnection(connectionString);
    }

    public async Task<PostgresConnection> OpenAsync() { if(!isOpened) await connection.OpenAsync(); isOpened = true; return this; }
    public NpgsqlCommand CreateCommand(string command) => new NpgsqlCommand(command, this.connection);

    public void Dispose()
    {
        connection.Dispose();
    }
}