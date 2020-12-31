using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBotBase.Logic.Event;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;

namespace DiscordBotBase.Listeners
{
    public class ReadyListener
    {
        [DiscordEvent(EventType.Ready)]
        public async Task ReadyAsync(BotShard shard, DiscordSocketClient client)
        {
            await client.SetGameAsync(
                name: $"over {shard.Config.ShardCount} shard" + (shard.Config.ShardCount > 1 ? "s!" : "!"),
                type: ActivityType.Watching
                );

            //await client.SetStatusAsync(UserStatus.Invisible);
            Console.WriteLine($"Ready! >> {client.CurrentUser}");
            Debug.WriteLine($"Ready! >> {client.CurrentUser}");
        }
    }
}
