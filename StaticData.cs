// using System;
// using System.Linq;
// using Discord;
// using Discord.WebSocket;
// using MCPanelBot;
// using MCPanelBot.Pterodactyl;
// using MCPanelBot;
// using Newtonsoft.Json;
// using WebSocketSharp;

// public class StaticData
// {
//     public static StaticData Data { get; private set; }
//     public static void Load()
//     {
//         if(!File.Exists("_resources/static.json")) { Data = new(); return; }
//         Data = JsonConvert.DeserializeObject<StaticData>(File.ReadAllText("_resources/static.json"))!;
//     }

//     public static void Save()
//     {
//         File.WriteAllText("_resources/static.json", JsonConvert.SerializeObject(Data));
//     }


//     public List<ConsoleChannel> ConsoleChannels { get; set; } = new();
//     public Dictionary<ulong, ServerConfig> ServerConfigs { get; set; } = new();
//     public static ServerConfig GetOrCreateServerConfig(ulong guildId)
//     {
//         if(!Data.ServerConfigs.ContainsKey(guildId)) Data.ServerConfigs[guildId] = new();
//         Save();
//         return Data.ServerConfigs[guildId];
//     }

//     private StaticData() {}
// }

// public class ConsoleChannel
// {
//     public string ServerId { get; set; }
//     public ulong OwnerId { get; set; }
//     public ulong ConsoleChannelId { get; set; }

//     [JsonIgnore] WebSocket Client { get; set; }

//     [JsonConstructor]
//     public ConsoleChannel(string serverId, ulong ownerId, ulong consoleChannelId)
//     {
//         ServerId = serverId;
//         OwnerId = ownerId;
//         ConsoleChannelId = consoleChannelId;

//         //Init();
//     }

//     private async void OnMessage(object? o, MessageEventArgs e)
//     {
//         // Console.WriteLine(e.Data);
//         // var msg = JsonConvert.DeserializeObject<WebSocketMessage>(e.Data)!;
//         // if(msg.Event == WSEvent.TokenExpiring || msg.Event == WSEvent.TokenExpired)
//         // {
//         //     var ws = await PterodactylClient.Key(UserData.Get(OwnerId)!.Key).GetWebsocket(ServerId);
//         //     Auth(ws.Token);
//         //     return;
//         // }

//         // if(msg.Event == WSEvent.ConsoleOutput)
//         // {
//         //     var log = msg.Arguments[0];
//         //     var channel = Program.client.GetChannel(ConsoleChannelId) as SocketTextChannel;
//         //     var lastBotMessage = (await channel.GetMessagesAsync(3).FlattenAsync()).Last();
//         //     if(lastBotMessage is not null && lastBotMessage.Author.Id == Program.client.CurrentUser.Id && lastBotMessage.Content.Length + log.Length < 2000)
//         //     {
//         //         var last = lastBotMessage.Content.Substring(0, lastBotMessage.Content.Length - 3);
//         //         last += log + "\n```";
//         //         await channel.ModifyMessageAsync(lastBotMessage.Id, (m) => m.Content = last);
//         //         return;
//         //     }

//         //     await channel.SendMessageAsync($"```\n{log}\n```");

//         //     return;
//         // }
//     }

//     private async void Init()
//     {
//         // var ws = await PterodactylClient.Key(UserData.Get(OwnerId)!.Key).GetWebsocket(ServerId);
//         // Client = new WebSocket(ws.Url);
//         // Client.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
//         // Client.OnMessage += OnMessage;
//         // Client.Connect();
//         // Auth(ws.Token);
//     }

//     private void Auth(string newToken)
//     {
//         var msg = new WebSocketMessage();
//         msg.SetEvent(WSEvent.Auth);
//         msg.Arguments = new List<string>() { newToken };
//         var str = JsonConvert.SerializeObject(msg);
//         Client.Send(str);
//     }
// }