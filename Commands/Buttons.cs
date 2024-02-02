using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Elephanel.Pterodactyl;
using Newtonsoft.Json;

namespace Elephanel.Commands;

public static class ButtonHandler
{
    public static async Task Handler(SocketMessageComponent component)
    {
        // if(!component.Data.CustomId.StartsWith("SERVER_")) return;
        // Console.WriteLine(component.Data.CustomId);
        // var args = component.Data.CustomId.Split("_");
        // var action = args[1];
        // var id = args[2];

        // var userData = UserData.Get(component.User.Id);
        // if(userData is null) return;
        // if(!userData.Categories.Any(c => c.ServerId == id)) return;

        // var client = new PterodactylClient(userData.Key);
        // switch(action)
        // {
        //     case "start":
        //         await client.PowerServer(id, PowerMode.Start);
        //         break;
        //     case "stop":
        //         await client.PowerServer(id, PowerMode.Stop);
        //         break;
        //     case "restart":
        //         await client.PowerServer(id, PowerMode.Stop);
        //         break;
        //     case "kill":
        //         await client.PowerServer(id, PowerMode.Kill);
        //         break;
        // }

        // await component.RespondAsync("Success!", ephemeral: true);
    }
}