using System.Data;
using Elephanel;
using Newtonsoft.Json;

public class ServerConfig
{
    public bool ShowPanelUrl { get; set; }
    public bool UsersCanManageAccounts { get; set; }
    public bool UsersCanAccessApiToken { get; set; }
    public bool ShowAccountName { get; set; }
    public bool DisplayAccountOnOtherServers { get; set; }
    public bool DisplaySeparateAccounts { get; set; }

    public static async Task<ServerConfig> GetOrCreate(ulong guildId)
    {
        using var connection = await PostgresConnection.Create().OpenAsync();
        var serverConfig = new ServerConfig();
        var cmd = connection.CreateCommand($"SELECT * FROM discordserverconfigs WHERE id = @id");
        cmd.Parameters.AddWithValue("id", (long)guildId);
        var reader = await cmd.ExecuteReaderAsync();
        string json = "";
        if(!reader.HasRows)
        {
            cmd.Cancel();
            await reader.CloseAsync();
            json = JsonConvert.SerializeObject(serverConfig);   
            var icmd = connection.CreateCommand("INSERT INTO discordserverconfigs ( id, json ) VALUES ( @id, @json )");
            icmd.Parameters.AddWithValue("id", (long)guildId);
            icmd.Parameters.AddWithValue("json", json);
            await icmd.ExecuteNonQueryAsync();
            return serverConfig;
        }

        await reader.ReadAsync();
        json = reader.GetFieldValue<string>("json");
        serverConfig = JsonConvert.DeserializeObject<ServerConfig>(json)!;
        return serverConfig;
    }

    public static async Task Update(ulong guildId, ServerConfig config)
    {
        config ??= new ServerConfig();
        using var connection = await PostgresConnection.Create().OpenAsync();
        var cmd = connection.CreateCommand($"UPDATE discordserverconfigs SET json = @json WHERE id = @id");
        cmd.Parameters.AddWithValue("id", (long)guildId);
        cmd.Parameters.AddWithValue("json", JsonConvert.SerializeObject(config));
        await cmd.ExecuteNonQueryAsync();
    }
}