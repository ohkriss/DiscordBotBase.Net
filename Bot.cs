using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using DiscordBotBase.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotBase.Entities;

namespace DiscordBotBase
{
    public class Bot
    {
        public static IServiceProvider Services { get; private set; }
        public Config Config { get; private set; }
        public DiscordSocketClient Client { get; private set; }
        public CommandService Command { get; private set; }

        public async Task InitializeAsync(string[] args)
        {
            if (File.Exists("Config.json"))
            {
                Config = JsonSerializer.Deserialize<Config>(File.ReadAllText("Config.json"));

                if (string.IsNullOrEmpty(Config.Token))
                {
                    Console.Write("Token > ");
                    Config.Token = Console.ReadLine();
                }

                if (string.IsNullOrEmpty(Config.Prefix))
                {
                    Console.Write("Command Prefix > ");
                    Config.Prefix = Console.ReadLine();
                }

                File.WriteAllText("Config.json", JsonSerializer.Serialize(Config, new() { WriteIndented = true }));
            }
            else
            {
                Console.Write("Token > ");
                Config = new() { Token = Console.ReadLine() };

                Console.Write("Command Prefix > ");
                Config.Prefix = Console.ReadLine();

                File.WriteAllText("Config.json", JsonSerializer.Serialize(Config, new() { WriteIndented = true }));
            }

            Command = new(new()
            {
                LogLevel = LogSeverity.Debug,
                DefaultRunMode = RunMode.Async
            });

            Client = new(new() { LogLevel = LogSeverity.Error, MessageCacheSize = 4096 });
            Client.Ready += async () =>
            {
                Console.WriteLine("Successfully logged in!");

                await Client.SetStatusAsync(UserStatus.DoNotDisturb);
            };

            Services = BuildServices();
            Services.GetRequiredService<CommandService>().Log += async (log) =>
            {
                await Console.Out.WriteLineAsync(log.Message);
            };

            try
            {
                await Client.LoginAsync(TokenType.Bot, Config.Token, false);
                await Client.StartAsync();

                await Services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            }
            catch (HttpException e)
            {
                Console.WriteLine(e.Message);
            }

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider BuildServices()
            => new ServiceCollection()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<InteractiveService>()
            .BuildServiceProvider();
    }
}
