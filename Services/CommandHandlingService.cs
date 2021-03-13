using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotBase.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotBase.Services
{
    class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly Config _config;

        public CommandHandlingService(IServiceProvider service, DiscordSocketClient discord, CommandService command, Config config)
        {
            _discord = discord;
            _commands = command;
            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
            _services = service;
            _config = config;
        }

        public async Task InitializeAsync()
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (rawMessage is not SocketUserMessage message) return;
            if (message.Source != MessageSource.User) return;

            int argPos = 0;

            if (!(message.HasStringPrefix(_config.Prefix, ref argPos) ||
                message.HasMentionPrefix(_discord.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) return;

            if (result.IsSuccess) return;

            await Console.Out.WriteLineAsync($"Error: {result}");
        }
    }
}
