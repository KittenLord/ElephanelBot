namespace Elephanel;

public class DiscordAccount
{
    [SqlColumn("discordid")] public long _DiscordId { get; private set; }
    public ulong DiscordId
    {
        get => (ulong)_DiscordId;
        set => _DiscordId = (long)value;
    }
    [SqlColumn("tokens")] public List<string> Tokens { get; private set; }

    public DiscordAccount(ulong discordId, List<string> tokens)
    {
        DiscordId = discordId;
        Tokens = tokens;
    }
}