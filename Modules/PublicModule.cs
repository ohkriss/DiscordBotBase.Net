using DiscordBotBase.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using DiscordBotBase.Attributes;

namespace DiscordBotBase.Modules
{
    public class PublicModule : InteractiveBase<SocketCommandContext>
    {
        [Command("ping")]
        [Cooldown(1, 5, CooldownBucketType.Guild, ErrorMessage = "Cooldown exception")]
        public async Task PingAsync()
        {
            await ReplyAsync($"Pong! `{Context.Client.Latency}ms`");
        }
    }
}
