using System.Text.Json.Serialization;
using Elephanel;
using Elephanel.Pterodactyl;

public class PterodactylToken
{
    [SqlColumn("tokenid")] public string TokenId { get; private set; }
    [SqlColumn("accountid")] public string AccountId { get; private set; }
    [SqlColumn("panelurl")] public string PanelUrl { get; private set; }
    [SqlColumn("apikey")] public string ApiKey { get; private set; }
    [SqlColumn("issuerid")] public long IssuerId { get; private set; }
    [SqlColumn("whitelistMode")] public bool IsWhitelist { get; private set; }
    [SqlColumn("serverlist")] public List<string> ServerFilter { get; private set; }

    public PterodactylToken(string tokenId, string accountId, string panelUrl, string apiKey, long issuerId, bool whitelistMode, List<string> serverFilter)
    {
        TokenId = tokenId;
        AccountId = accountId;
        PanelUrl = panelUrl;
        ApiKey = apiKey;
        IssuerId = issuerId;
        IsWhitelist = whitelistMode;
        ServerFilter = serverFilter;
    }
}