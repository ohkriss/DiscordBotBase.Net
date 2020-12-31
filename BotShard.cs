using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotBase.Entities;
using DiscordBotBase.Logic.Event;
using DiscordBotBase.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotBase
{
    public class BotShard
    {
        public static IServiceProvider Services { get; private set; }

        public int ShardId { get; private set; }

        public DiscordSocketClient Client { get; private set; }
        public CommandService Command { get; private set; }
        public Config Config { get; }

        public Bot Parent;

        public BotShard(Config config, int id, Bot bot)
        {
            Config = config;
            ShardId = id;
            Parent = bot;
        }

        internal async void Initialize()
        {
            Client = new(new()
            {
                LogLevel = LogSeverity.Debug,
                TotalShards = Config.ShardCount,
                ShardId = ShardId,
                MessageCacheSize = 2048
            });

            Command = new(new()
            {
                LogLevel = LogSeverity.Debug,
                DefaultRunMode = RunMode.Async
            });

            Services = BuildServices();

            Services.GetRequiredService<CommandService>().Log += async (log) =>
            {
                await Console.Out.WriteLineAsync(log.Message);
                Debug.WriteLine(log.Message);
            };

            await Services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            DiscordEventHandler.InstallListeners(Client, this);

            await Client.LoginAsync(TokenType.Bot, Config.Token, false);
        }

        public Task RunAsync() =>
            Client.StartAsync();

        internal async Task DisconnectAndDispose()
        {
            await Client.StopAsync();
            Client.Dispose();
        }

        private ServiceProvider BuildServices()
            => new ServiceCollection()
            .AddSingleton(this)
            .AddSingleton(Config)
            .AddSingleton(Client)
            .AddSingleton(Command)
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<InteractiveService>()
            .BuildServiceProvider();
    }
}
