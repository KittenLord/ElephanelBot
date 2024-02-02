using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Elephanel.Installers;
using Elephanel.Pterodactyl;
using Modrinth;
using Elephanel;
using Newtonsoft.Json;

[Group("install", "Install a mod or plugin on your server")]
public class InstallCommandGroup : InteractionModuleBase
{
    public enum ModLoader
    {
        Forge,
        Fabric,
        Quilt,
        Neoforge
    }

    [SlashCommand("mod", "Install a mod")]
    public async Task InstallModCommand(string modName, ModLoader modLoader, string version)
    {
        try{





        var client = ModrinthModInstaller.Client;
        
        var f = new FacetCollection();
        f.Add(Facet.ProjectType(Modrinth.Models.Enums.Project.ProjectType.Mod));
        f.Add(Facet.Category(modLoader.ToString().ToLower()));

        var searchResults = (await client.Project.SearchAsync(modName, facets: f, limit: 99999));

        var id = Guid.NewGuid().ToString();

        var index = 0;
        var menu = new SelectMenuBuilder().WithCustomId(id + "mnu");
        foreach(var mod in searchResults.Hits.Take(25)) menu.AddOption(mod.Title, index++.ToString(), string.Join(", ", mod.Categories));

        var component = new ComponentBuilder()
            .WithSelectMenu(menu, 0)
            .WithButton("I can't find my mod", id + "fnd",   ButtonStyle.Danger,    row: 1)
            .WithButton("Cancel",              id + "ccl", ButtonStyle.Secondary, row: 1);

        var loop = true;
        var action = "";
        var handler = async (SocketMessageComponent c) => 
        {
            if(!c.Data.CustomId.StartsWith(id)) return;
            await c.DeferAsync();
            loop = false;
            action = new string(c.Data.CustomId.TakeLast(3).ToArray());
            index = int.Parse(c.Data.Values.First());
        };

        await Context.Interaction.DeferAsync();
        await Context.Channel.SendMessageAsync("", components: component.Build());
        Program.client.SelectMenuExecuted += handler;
        Program.client.ButtonExecuted += handler;
        while(loop){await Task.Delay(5);}
        Program.client.ButtonExecuted -= handler;
        Program.client.SelectMenuExecuted += handler;

        await Context.Channel.SendMessageAsync(action);






        }catch(Exception e){Console.WriteLine(e);}
    }

    [SlashCommand("plugin", "Install a plugin")]
    public async Task InstallPluginCommand(string pluginName, string version)
    {
        await Context.Channel.SendMessageAsync("aboba");
    }
}
