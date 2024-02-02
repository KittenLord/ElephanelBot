using System;
using System.Runtime.CompilerServices;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Modrinth;
using Modrinth.Extensions;
using Modrinth.Helpers;
using Elephanel;
using Elephanel;
using Elephanel.Pterodactyl;
using System.Net.Http.Headers;
using System.Data.Common;

namespace Elephanel.Installers;

public static class ModrinthModInstaller
{
    public static ModrinthClient Client;

    static ModrinthModInstaller()
    {
        var config = new ModrinthClientConfig
        {
            UserAgent = "KittenLord"
        };
        Client = new ModrinthClient(config);
    }

    public static async Task<List<(string name, string id, List<string> categories)>> Search(string query)
    {
        return (await Client.Project.SearchAsync(query, Modrinth.Models.Enums.Index.Relevance)).Hits.Select(hit => (name: hit.Title, id: hit.ProjectId, categories: hit.Categories.ToList())).ToList();
    }

    // public static async Task<string> GetDownloadLink(SocketCommandContext context, string id, string version)
    // {
        // var versions = await Client.Version.GetProjectVersionListAsync(id, gameVersions: new string[] { version });
        // if(version.Length <= 0) { await context.Channel.SendMessageAsync("Something went wrong"); return ""; }

        // var menuId = Guid.NewGuid().ToString();
        // var menu = new SelectMenuBuilder()
        //     .WithMaxValues(1)
        //     .WithMinValues(1)
        //     .WithCustomId(menuId);

        // int index = 0;
        // versions.ToList().ForEach(v => { if(index < 20) menu.AddOption(v.Name, (index++).ToString(), v == versions.First() ? "Latest" : "-"); });

        // var msg = await context.Channel.SendMessageAsync("aboba", components: new ComponentBuilder().WithSelectMenu(menu).Build());

        // bool waiting = true;
        // async Task Handler(SocketMessageComponent component)
        // {
        //     if(component.Data.CustomId != menuId) return;
        //     await component.DeferAsync();
        //     waiting = false;
        //     index = int.Parse(component.Data.Values.First());
        // }

        // Program.client.SelectMenuExecuted += Handler;
        // while(waiting){await Task.Delay(5);}
        // Program.client.SelectMenuExecuted -= Handler;
        
        // menu.IsDisabled = true;
        // menu.Options.Find(o => o.Value == index.ToString())!.IsDefault = true;
        // await msg.ModifyAsync(m => m.Components = new ComponentBuilder().WithSelectMenu(menu).Build());

        // var selectedVersion = versions[index];
        // var urls = selectedVersion.Files.Select(f => f.Url).ToList();


        // var userData = UserData.Get(context.User.Id)!;
        // var userState = UserState.Get(context.User.Id)!;
        // var ptClient = PterodactylClient.Key(userData.Key);
        // var server = await ptClient.GetServer(userState.SelectedServerIndex);
        // var serverId = server.Attributes.Identifier;


        // var folderId = Guid.NewGuid().ToString();
        // Directory.CreateDirectory($"temp/{folderId}");

        // foreach(var url in urls) await context.Channel.SendMessageAsync(url);
        // var httpClient = new HttpClient();
        // Parallel.ForEach(urls, async url => {
        //     var fileName = url.Split("/").Last();
        //     var fs = new FileStream($"temp/{folderId}/{fileName}", FileMode.CreateNew);
        //     var response = await httpClient.GetAsync(url);
        //     await response.Content.CopyToAsync(fs);
        //     fs.Close();

        //     await ptClient.UploadFile(serverId, $"temp/{folderId}/{fileName}", fileName, "mods");
        // });

        // return "";
    // }
}
