using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using Elephanel;
using Elephanel.Commands;
using Elephanel.Pterodactyl;
using Discord.Interactions;

namespace Elephanel
{
    public class Program
    {
        public struct BotConfig
        {
            public string Token { get; private set; }
            public string Prefix { get; private set; }
            public string PostgresConnectionString { get; private set; }

            public BotConfig(string token, string prefix, string postgresConnectionString)
            {
                Token = token;
                Prefix = prefix;
                PostgresConnectionString = postgresConnectionString;
            }
        }

        public static BotConfig botConfig { get; private set; }

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public static DiscordSocketClient client;
        private CommandService commands;
        private InteractionService interactions;
        private IServiceProvider services;

        public async Task MainAsync()
        {
            botConfig = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(@"_resources/config.json"));

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                LogLevel = LogSeverity.Debug
            });

            client.Log += log =>
            {
                Console.WriteLine(log.ToString());
                return Task.CompletedTask;
            };

            client.ButtonExecuted += ButtonHandler.Handler;
            
            //services = new ServiceCollection()
            //    .AddSingleton(client)
            //    .BuildServiceProvider();

            commands = new CommandService();
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            interactions = new InteractionService(client.Rest);
            client.Ready += async() =>
            {
                await interactions.AddModulesAsync(Assembly.GetEntryAssembly(), services);
                await interactions.RegisterCommandsToGuildAsync(1198970161714188381);
            };

            client.InteractionCreated += HandleInteractionAsync;
            client.MessageReceived += HandleCommandAsync;

            await client.LoginAsync(TokenType.Bot, botConfig.Token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task HandleInteractionAsync(SocketInteraction interaction)
        {
            var context = new InteractionContext(client, interaction, interaction.Channel);
            try{
            var result = await interactions.ExecuteCommandAsync(context, services);
            if(!result.IsSuccess)
            {
                Console.WriteLine(result.Error.Value);
                Console.WriteLine(result.ErrorReason);
            }
            }catch(Exception e){Console.WriteLine(e);}
        }

        public async Task HandleCommandAsync(SocketMessage m)
        {
            if (m is not SocketUserMessage msg) return;
            if (msg.Author.IsBot) return;

            int argPos = 0;
            if (!msg.HasStringPrefix(botConfig.Prefix, ref argPos)) return;

            var context = new SocketCommandContext(client, msg);

            var result = await commands.ExecuteAsync(context, argPos, services);
            if(!result.IsSuccess)
            {
                Console.WriteLine(result.Error.Value);
                Console.WriteLine(result.ErrorReason);
            }
        }
    }
}