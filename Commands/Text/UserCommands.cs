using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Elephanel.Installers;
using Elephanel.Pterodactyl;
using Newtonsoft.Json;

namespace Elephanel.Commands;

public class UserCommands : ModuleBase<SocketCommandContext>
{
    // [Command("servers")]
    // public async Task ServerListCommand()
    // {
    //     var data = UserData.Get(Context.User.Id);
    //     if(data is null) return;

    //     string answer = "**YOUR SERVERS**\n```css\n";
    //     var list = await PterodactylClient.Key(data.Key).GetServers();
    //     for(int i = 0; i < list.Count; i++)
    //     {
    //         var server = list[i];
    //         answer += $"[{i}]: \"{server.Attributes.Name}\", #{server.Attributes.Identifier}\n";
    //     }
    //     answer += "```";
    //     await Context.Channel.SendMessageAsync(answer);
    // }

    // [Command("info")]
    // public async Task ServerInfoCommand()
    // {
    //     var state = UserState.Get(Context.User.Id);
    //     await ServerInfoCommand(state.SelectedServerIndex);
    // }

    // [Command("info")]
    // public async Task ServerInfoCommand(int index)
    // {
    //     var data = UserData.Get(Context.User.Id);
    //     if(data is null) return;
    //     var server = await PterodactylClient.Key(data.Key).GetServer(index);
    //     var msg = server.Attributes.Name;
    //     await Context.Channel.SendMessageAsync(msg);
    // }

    // [Command("start")]
    // public async Task ServerStartCommand()
    // {
    //     var index = UserState.Get(Context.User.Id).SelectedServerIndex;
    //     var data = UserData.Get(Context.User.Id);
    //     if(data is null) return;
    //     var client = new PterodactylClient(data.Key);
    //     var server = await client.GetServer(index);
    //     await client.PowerServer(server.Attributes.Identifier, PowerMode.Start);
    // }

    // [Command("command")]
    // public async Task ServerCommandCommand([Remainder]string command)
    // {
    //     var index = UserState.Get(Context.User.Id).SelectedServerIndex;
    //     var data = UserData.Get(Context.User.Id);
    //     if(data is null) return;
    //     var client = new PterodactylClient(data.Key);
    //     var server = await client.GetServer(index);
    //     await client.SendCommand(server.Attributes.Identifier, command);
    // }

    // [Command("install")]
    // public async Task InstallCommand(string mode, [Remainder]string name)
    // {
    //     try{
    //     if(mode == "mod") await InstallMod(name);
    //     else if(mode == "plugin") await InstallPlugin();
    //     }catch(Exception e){Console.WriteLine(e);}
    // }

    // private async Task InstallMod(string modName)
    // {
    //     var results = await ModrinthModInstaller.Search(modName);

    //     var menu = new SelectMenuBuilder()
    //         .WithCustomId("dwaidnaw");
        
    //     foreach(var result in results)
    //     {
    //         if(menu.Options.Count < 20)
    //         menu.AddOption(result.name, result.id, string.Join(", ", result.categories));
    //     }

    //     var component = new ComponentBuilder()
    //         .WithSelectMenu(menu);

    //     await Context.Channel.SendMessageAsync("Search results", components: component.Build());
    // }

    // private async Task InstallPlugin()
    // {

    // }
}