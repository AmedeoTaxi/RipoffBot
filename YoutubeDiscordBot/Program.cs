using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using YoutubeDiscordBot.Services;
using YoutubeDiscordBot.Services.Logging;
using System.Configuration;

namespace YoutubeDiscordBot
{
    internal class Program
    {
        public static async Task Main(string[] args) => await new Program().MainAsync();

        public async Task MainAsync()
        {
            #region Init

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ILogger, Logger>();
            serviceCollection.AddSingleton<IYoutubeService, YoutubeService>();
            serviceCollection.AddSingleton<IDiscordClient, DiscordSocketClient>(client => new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            }));
            serviceCollection.AddSingleton(typeof(CommandService));
            serviceCollection.AddSingleton<ICommandHandlerService, CommandHandlerService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger>();
            var commandHandler = serviceProvider.GetService<ICommandHandlerService>();

            var discordClient = serviceProvider.GetService<IDiscordClient>() as DiscordSocketClient;

            discordClient.Log += logger.DiscordLog;

            var token = ConfigurationManager.AppSettings["DiscordToken"];

            await commandHandler.InstallCommandsAsync();

            #endregion

            #region Login

            await discordClient.LoginAsync(TokenType.Bot, token);
            await discordClient.StartAsync();

            #endregion

            await Task.Delay(-1);
        }
    }
}