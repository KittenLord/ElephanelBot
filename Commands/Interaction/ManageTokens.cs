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
using System.Reactive.Linq;

[Group("token", "Manage user accounts")]
public class AccountManagementGroup : InteractionModuleBase
{
    [SlashCommand("create-add", "Create and add a token to a user")]
    public async Task AddTokenCommand(IUser user, string panelUrl, string apiKey)
    {   
        // Cant be bothered to negate this expression lol
        if (user is SocketGuildUser gu && 
           (gu.GuildPermissions.Administrator
        || (gu.Id == Context.User.Id && ServerConfig.GetOrCreate(Context.Guild.Id).Result.UsersCanManageAccounts))){}
        else { return; }
        if(!panelUrl.EndsWith("/")) panelUrl += "/";


        try{
        var pt = new PterodactylClient(panelUrl, apiKey);
        var discordAccountTable = new SqlTable<DiscordAccount>("discordaccounts");
        var pterodactylTokenTable = new SqlTable<PterodactylToken>("pterodactyltokens");

        var tokenId = Hash.SHA256String(apiKey + panelUrl).Substring(0, 32);
        var accountId = Hash.SHA256String(pt.GetAccount().Result.Attributes.Id.ToString() + panelUrl).Substring(0, 32);
        var token = new PterodactylToken(tokenId, accountId, panelUrl, apiKey, (long)Context.Guild.Id, false, new());
        var discordAccount = discordAccountTable.SelectWhere("@id = discordid", new SqlParameters().Add("id", Context.User.Id)).Result.FirstOrDefault();
        if(discordAccount is null) { discordAccount = new DiscordAccount(Context.User.Id, []); await discordAccountTable.Insert(discordAccount); } 

        // Not sure if user can add multiple tokens for the same account
        if(discordAccount.Tokens.Contains(tokenId)) { return; }

        var valid = await pt.ValidateAccount();

        if(!valid) { return; }

        await Context.Interaction.RespondAsync("Account was successfully added!");

        discordAccount.Tokens.Add(tokenId);
        await discordAccountTable.UpdateWhere("tokens = @tokens", "discordid = @id", new SqlParameters().Add("id", Context.User.Id).Add("tokens", discordAccount.Tokens));
        if(pterodactylTokenTable.SelectWhere("@tokenid = tokenid", new SqlParameters().Add("tokenid", tokenId)).Result.Count == 0) 
            await pterodactylTokenTable.Insert(token);
        }catch(Exception e){Console.WriteLine(e);}
    }
}