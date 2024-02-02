using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Elephanel.Pterodactyl;
using Elephanel;
using Newtonsoft.Json;
using Npgsql;

namespace Elephanel.Commands
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task Test()
        {
        }

        // [Command("setup-user")]
        // public async Task SetupUser(SocketUser user, [Remainder]string key)
        // {
        //     var data = new UserData(key);
        //     UserData.Update(user.Id, data);
        //     await Context.Message.DeleteAsync();
        //     await Context.Channel.SendMessageAsync("Success");
        // }

        // [Command("setup-server")]
        // public async Task SetupServer(SocketUser user, string serverId)
        // {
        //     var data = UserData.Get(user.Id);
        //     if(data is null) { await Context.Channel.SendMessageAsync("This user hasn't been set up"); return; }
        //     var client = new PterodactylClient(data.Key);
        //     var server = await client.GetServer(serverId);
        //     if(server is null) { await Context.Channel.SendMessageAsync("Server with this id doesn't exist"); return; }

        //     var category = await Context.Guild.CreateCategoryChannelAsync($"{server.Attributes.Name}-{server.Attributes.Identifier}");
            
        //     var panelChannel = await Context.Guild.CreateTextChannelAsync("panel");
        //     await panelChannel.ModifyAsync(c => c.CategoryId = category.Id);

        //     var consoleChannel = await Context.Guild.CreateTextChannelAsync("console");
        //     await consoleChannel.ModifyAsync(c => c.CategoryId = category.Id);

        //     var categoryInfo = new ServerCategory(serverId, category.Id, consoleChannel.Id);
        //     data.Categories.Add(categoryInfo);
        //     UserData.Save();

        //     StaticData.Data.ConsoleChannels.Add(new ConsoleChannel(server.Attributes.Identifier, user.Id, consoleChannel.Id));
        //     StaticData.Save();

        //     var panelBuilder = new ComponentBuilder()
        //         .WithButton("Start", $"SERVER_start_{serverId}", ButtonStyle.Success)
        //         .WithButton("Restart", $"SERVER_restart_{serverId}", ButtonStyle.Secondary)
        //         .WithButton("Stop", $"SERVER_stop_{serverId}", ButtonStyle.Danger)
        //         .WithButton("Kill", $"SERVER_kill_{serverId}", ButtonStyle.Danger);
                
        //     await panelChannel.SendMessageAsync("Panel", components: panelBuilder.Build());
        // }
    }
}
