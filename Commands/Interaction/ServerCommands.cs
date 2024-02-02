using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Elephanel;
using Elephanel.Installers;
using Elephanel.Pterodactyl;
using Modrinth;
using Newtonsoft.Json;
using WebSocketSharp.Net;

[Group("server", "Manage your servers")]
public class ServerCommandsGroup : InteractionModuleBase
{


    [SlashCommand("list", "List all of your servers")]
    public async Task ListServersCommand()
    {
        try{
        var discordAccountTable = new SqlTable<DiscordAccount>("discordaccounts");
        var pterodactylTokenTable = new SqlTable<PterodactylToken>("pterodactyltokens");

        var discordAccount = discordAccountTable.SelectWhereOrCreate("discordid = @id", new SqlParameters().Add("id", (long)Context.User.Id), new DiscordAccount(Context.User.Id, new())).Result.First();
        if(discordAccount.Tokens.Count <= 0) { return; }



        var tokens = new List<PterodactylToken>();
        foreach(var tokenId in discordAccount.Tokens) tokens.AddRange(await pterodactylTokenTable.SelectWhere("tokenid = @tokenid", SqlParameters.AddInit("tokenid", tokenId)));
        if(tokens.Count <= 0) { return; }

        Dictionary<string, string> names = new Dictionary<string, string>();
        string GetAccountName(PterodactylToken token, ServerConfig config)
        {
            if(!config.DisplaySeparateAccounts && Context.Guild.Id == (ulong)token.IssuerId) return "[This Server]";
            if(!config.DisplaySeparateAccounts) return "[Other server]";
            if(config.ShowAccountName) return new PterodactylClient(token).GetAccount().Result.Attributes.Username;
            if(names.ContainsKey(token.AccountId)) return names[token.AccountId];
            names[token.AccountId] = $"Account {names.Count}";
            return names[token.AccountId];
        }

        var accounts = tokens
            .Select(token => (client: new PterodactylClient(token), token: token))
            .Where(pair => pair.client.ValidateAccount().Result)
            .Select(pair => (servers: pair.client.GetServers().Result, token: pair.token)) // TODO: server black/whitelisting
            .Where(pair => pair.servers.Count > 0)
            .SelectMany(pair => pair.servers.Select(server => (server: server, token: pair.token)))
            .GroupBy(pair => pair.server.Attributes.Uuid)
            .Select(group => (
                server: group.First().server, 
                tokens: group.Select(g => (
                    token: g.token, 
                    config: ServerConfig.GetOrCreate((ulong)g.token.IssuerId).Result))
                    .Where(token => !token.config.DisplayAccountOnOtherServers || Context.Guild.Id == (ulong)token.token.IssuerId)))
            .Select(pair => (
                server: pair.server,
                
                separatedPublic: pair.tokens.Where(token => token.config.DisplaySeparateAccounts && token.config.ShowAccountName),
                separatedPrivate: pair.tokens.Where(token => token.config.DisplaySeparateAccounts && !token.config.ShowAccountName),
                privateThisServer: pair.tokens
                    .Where(token => !token.config.DisplaySeparateAccounts && Context.Guild.Id == (ulong)token.token.IssuerId).Take(1),
                privateOtherServer: pair.tokens
                    .Where(token => !token.config.DisplaySeparateAccounts && Context.Guild.Id != (ulong)token.token.IssuerId).Take(1)))
            .Select(tuple => (
                server: tuple.server,

                tokens: tuple.separatedPublic.Concat(tuple.separatedPublic.Count() > 0 ? [] : 
                    tuple.separatedPrivate.Concat(tuple.separatedPrivate.Count() > 0 ? [] :
                        tuple.privateThisServer.Concat(tuple.privateThisServer.Count() == 0 ? [] :
                            tuple.privateOtherServer)))))
            .Select(tuple => (
                server: tuple.server, 
                token: tuple.tokens.First().token, 
                accounts: string.Join(", ", tuple.tokens.Select(token => GetAccountName(token.token, token.config)))))
            .GroupBy(tuple => tuple.accounts);

        var settings = await UserSettings.GetOrCreate(Context.User.Id);
        string message = "";

        foreach(var account in accounts)
        {
            message += account.Key + "\n";
            foreach(var server in account)
            {
                message += $"\t{server.server.Attributes.Name} - {server.server.Attributes.Identifier}";
                if(server.server.Attributes.Uuid == settings.SelectedServerId && server.token.AccountId == settings.SelectedAccountId) 
                    message = "+ " + message;
            }
        }

        await Context.Interaction.RespondAsync($"```diff\n{message}\n```");
        }catch(Exception e){Console.WriteLine(e);}
    }


//     [SlashCommand("power", "Change server's power status")]
//     public async Task PowerServerCommand(MCPanelBot.Pterodactyl.PowerMode powerMode)
//     {
//         var userData = UserData.GetOrCreate(Context.User.Id);
//         var selected = await GetOrCreateSelected(Context.User.Id);
//         if(selected is null) { return; }

//         var account = userData.Accounts.Find(a => a.AccountId == selected.AccountId);
//         if(account is null) { return; }

//         await new PterodactylClient(account).PowerServer(selected.ServerId, powerMode);
//     }

//     [SlashCommand("setup-category", "Set up a channel category to control a server")]
//     [RequireUserPermission(GuildPermission.Administrator)]
//     public async Task SetupServerCategoryCommand()
//     {
        
//     }
}