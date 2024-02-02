using Newtonsoft.Json;

namespace Elephanel.Pterodactyl;

public enum PowerMode
{
    Start,
    Stop,
    Restart,
    Kill
}

public class PterodactylClient
{
    private string baseUrl;
    private string key;

    private HttpClient httpClient;
    public PterodactylClient(PterodactylToken token) : this(token.PanelUrl, token.ApiKey) {}
    public PterodactylClient(string baseUrl, string key)
    {
        if(!baseUrl.EndsWith("/")) baseUrl += "/";

        this.baseUrl = baseUrl;
        this.key = key;
        httpClient = new HttpClient();
    }

    public async Task<bool> ValidateAccount()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, baseUrl + "api/client");
        req.Headers.Add("Authorization", $"Bearer {key}");
        req.Headers.Add("Accept", "application/json");
        var response = await httpClient.SendAsync(req);
        return response.IsSuccessStatusCode;
    }

    public async Task<string> SendRequest(HttpMethod method, string url, string json, string key)
    {
        // TODO: Add ratelimiting

        // Console.WriteLine(url);
        // Console.WriteLine(json);
        // Console.WriteLine(key);

        HttpRequestMessage msg = new HttpRequestMessage(method, new Uri(baseUrl + "api/" + url));
        msg.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        msg.Headers.Add("Authorization", $"Bearer {key}");
        msg.Headers.Add("Accept", "application/json");
        var response = await httpClient.SendAsync(msg).ConfigureAwait(false);
        Console.WriteLine(response.StatusCode);
        

        return await response.Content.ReadAsStringAsync();
    }


    public async Task<List<Server>> GetServers()
    {
        var response = await SendRequest(HttpMethod.Get, "client", "", key);
        var list = JsonConvert.DeserializeObject<ServerCollection>(response)!.Data;
        return list;
    }

    public async Task<Account> GetAccount()
    {
        var response = await SendRequest(HttpMethod.Get, "client/account", "", key);
        return JsonConvert.DeserializeObject<Account>(response)!;
    }

    public async Task<Server> GetServer(int index)
    {
        return (await GetServers())[index];
    }

    public async Task<Server?> GetServer(string id)
    {
        return (await GetServers()).Find(s => s.Attributes.Identifier == id);
    }

    public async Task PowerServer(string id, PowerMode mode = PowerMode.Start)
    {
        await SendRequest(HttpMethod.Post, $"client/servers/{id}/power", $"{{ \"signal\":\"{mode.ToString().ToLower()}\" }}", key);
    }

    public async Task SendCommand(string id, string command)
    {
        await SendRequest(HttpMethod.Post, $"client/servers/{id}/command", $"{{ \"command\":\"{command}\" }}", key);
    }

    public class UploadUrlData
    {
        [JsonProperty("attributes")]
        public UploadUrlData2 Data { get; set; }
    }
    public class UploadUrlData2
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public async Task<string> GetUploadUrl(string id)
    {
        var response = await SendRequest(HttpMethod.Get, $"client/servers/{id}/files/upload", "", key);
        var data = JsonConvert.DeserializeObject<UploadUrlData>(response)!;
        return data.Data.Url;
    }

    public async Task UploadFile(string id, string localFilePath, string fileName, string directoryName="/")
    {
        using var httpClient = new HttpClient();
        var uploadUrl = await this.GetUploadUrl(id) + $"&directory={directoryName}";

        var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(File.ReadAllBytes(localFilePath)), "files", fileName);
        var hq = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
        hq.Content = content;
        var uploadResponse = await httpClient.SendAsync(hq);
        Console.WriteLine(await uploadResponse.Content.ReadAsStringAsync());
    }

    public class WSInfo
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("socket")]
        public string Url { get; set; }
    }

    public class WSInfoWrapper
    {
        public WSInfo data { get; set; }
    }

    public async Task<WSInfo> GetWebsocket(string serverId)
    {
        var response = await SendRequest(HttpMethod.Get, $"client/servers/{serverId}/websocket", "", key);
        var ws = JsonConvert.DeserializeObject<WSInfoWrapper>(response)!;
        return ws.data;
    }
}
