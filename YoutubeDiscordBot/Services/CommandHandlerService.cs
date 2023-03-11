using Discord;
using Discord.Commands;
using Discord.WebSocket;
using YoutubeDiscordBot.Services.Logging;
using System.Reflection;

namespace YoutubeDiscordBot.Services
{
    internal interface ICommandHandlerService
    {
        public Task InstallCommandsAsync();
        public Task HandleCommandsAsync(SocketMessage messageParam);
    }
    internal class CommandHandlerService : ICommandHandlerService
    {
        private readonly DiscordSocketClient? _client;
        private readonly CommandService _commands;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerService(ILogger logger, IDiscordClient client, CommandService commands, IServiceProvider serviceProvider)
        {
            _client = client as DiscordSocketClient;
            _commands = commands;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandsAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        public async Task HandleCommandsAsync(SocketMessage messageParam)
        {
            if (!messageParam.Author.IsBot)
            {
                _logger.WriteLog("Received message: " + messageParam.Content);
            }

            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            //if (!message.HasCharPrefix('!', ref argPos))
            //{
            //    return;
            //}

            if (message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            if (message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _serviceProvider);
        }
    }
}
