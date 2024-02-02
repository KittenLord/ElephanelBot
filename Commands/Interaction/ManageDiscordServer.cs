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

[Group("config", "Server configuration")]
public class DiscordServerManagementCommands : InteractionModuleBase
{
    private string DisplayJsonWithSmartLineNumbers(string[] lines)
    {
        var lineLabelWidth = lines.Length.ToString().Length + 1;
        string message = "";
        string EnforceMinWidth(string text, int width) { while(text.Length < width) text += " "; return text; }

        int lastLineNumber = 0;
        for(int i = 0; i < lines.Length; i++) 
        { 
            message += EnforceMinWidth(lines[i].Contains("\"") ? lastLineNumber++.ToString() : "", lineLabelWidth) + "| " + lines[i] + "\n"; 
        }

        return message;
    }

    [SlashCommand("view", "View the server configuration")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task ConfigurateServerCommand()
    {
        var config = await ServerConfig.GetOrCreate(Context.Guild.Id);
        var configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
        var configLines = configJson.Split("\n");
        var display = DisplayJsonWithSmartLineNumbers(configLines);
        await Context.Interaction.RespondAsync($"```json\n{display}\n```", ephemeral: true);
    }

    [SlashCommand("set", "Set a value in the server configuration")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task SetConfigValueCommand(uint lineNumber, string value)
    {
        var config = await ServerConfig.GetOrCreate(Context.Guild.Id);
        var configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
        var configLines = configJson.Split("\n");
        
        int lineIndex = -1;
        for(int i = 0; i < configLines.Length; i++) 
        {
            if(configLines[i].Contains("\"")) lineIndex++;
            if(lineIndex == lineNumber) { lineIndex = i; break; }
            if(i == configLines.Length - 1) return;
        }

        var line = configLines[lineIndex];
        configLines[lineIndex] = line.Split(":")[0] + ": " + value + ",";
        configJson = string.Join("", configLines);

        configLines[lineIndex] = line.Split(":")[0] + ": >>>" + value + "<<<,";

        try { config = JsonConvert.DeserializeObject<ServerConfig>(configJson); if(config is null) throw new Exception(); }
        catch { await Context.Interaction.RespondAsync("bad", ephemeral: true); return; }

        var display = DisplayJsonWithSmartLineNumbers(configLines);

        await ServerConfig.Update(Context.Guild.Id, config);
        await Context.Interaction.RespondAsync($"```json\n{display}\n```", ephemeral: true);
    }
}