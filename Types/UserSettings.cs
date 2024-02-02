using Elephanel;
using Newtonsoft.Json;
using System.Data;

public class UserSettings
{
    public string SelectedAccountId { get; set; }
    public string SelectedServerId { get; set; }
    
    private static PostgresConnection connection;

    public static async Task<UserSettings> GetOrCreate(ulong userId) => await GetOrCreate(userId, new UserSettings());
    public static async Task<UserSettings> GetOrCreate(ulong userId, UserSettings defaultValue)
    {
        using var connection = await PostgresConnection.Create().OpenAsync();
        var userSettings = defaultValue;
        var cmd = connection.CreateCommand($"SELECT * FROM discordusersettings WHERE id = @id");
        cmd.Parameters.AddWithValue("id", (long)userId);
        var reader = await cmd.ExecuteReaderAsync();
        string json = "";
        if(!reader.HasRows)
        {
            cmd.Cancel();
            await reader.CloseAsync();
            json = JsonConvert.SerializeObject(userSettings);   
            var icmd = connection.CreateCommand("INSERT INTO discordusersettings ( id, json ) VALUES ( @id, @json )");
            icmd.Parameters.AddWithValue("id", (long)userId);
            icmd.Parameters.AddWithValue("json", json);
            await icmd.ExecuteNonQueryAsync();
            return userSettings;
        }

        await reader.ReadAsync();
        json = reader.GetFieldValue<string>("json");
        userSettings = JsonConvert.DeserializeObject<UserSettings>(json)!;
        return userSettings;
    }

    public static async Task Update(ulong userId, UserSettings settings)
    {
        settings ??= new UserSettings();
        using var connection = await PostgresConnection.Create().OpenAsync();
        var cmd = connection.CreateCommand($"UPDATE discordusersettings SET json = @json WHERE id = @id");
        cmd.Parameters.AddWithValue("id", (long)userId);
        cmd.Parameters.AddWithValue("json", JsonConvert.SerializeObject(settings));
        await cmd.ExecuteNonQueryAsync();
    }
}