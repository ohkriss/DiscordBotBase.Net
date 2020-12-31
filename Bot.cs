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
        public List<BotShard> Shards { get; private set; }
        public CancellationTokenSource CTS { get; private set; }


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

                if (Config.ShardCount < 1)
                {
                    Console.Write("Shard Count > ");
                    Config.ShardCount = int.Parse(Console.ReadLine());
                }

                File.WriteAllText("Config.json", JsonSerializer.Serialize(Config, new() { WriteIndented = true }));
            }
            else
            {
                Console.Write("Token > ");
                Config = new() { Token = Console.ReadLine() };

                Console.Write("Command Prefix > ");
                Config.Prefix = Console.ReadLine();

                Console.Write("Shard Count > ");
                Config.ShardCount = int.Parse(Console.ReadLine());

                File.WriteAllText("Config.json", JsonSerializer.Serialize(Config, new() { WriteIndented = true }));
            }

            Shards = new List<BotShard>();

            for (var i = 0; i < Config.ShardCount; i++)
            {
                var shard = new BotShard(Config, i, this);
                shard.Initialize();
                Shards.Add(shard);
            }


            foreach (var shard in Shards)
                await shard.RunAsync();

            CTS = new CancellationTokenSource();

            var token = CTS.Token;

            try
            {
                await Task.Delay(Timeout.Infinite, token);
            }
            catch (TaskCanceledException) { }

            foreach (var shard in Shards)
                await shard.DisconnectAndDispose();
        }
    }
}
