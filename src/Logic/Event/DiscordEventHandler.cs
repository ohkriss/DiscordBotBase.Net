using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DiscordBotBase.Logic.Event
{
    public class DiscordEventHandler
    {
        private static IEnumerable<ListenerMethod> ListenerMethods { get; set; }

        public static void InstallListeners(DiscordSocketClient client, BotShard bot)
        {
            ListenerMethods =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                from m in t.GetMethods()
                let attribute = m.GetCustomAttribute(typeof(DiscordEventAttribute), true)
                where attribute != null
                select new ListenerMethod { Method = m, Attribute = attribute as DiscordEventAttribute, Type = t };

            foreach (var listener in ListenerMethods)
                listener.Attribute.Register(bot, client, listener);
        }
    }

    public class ListenerMethod
    {
        public Type Type { get; internal set; }
        public MethodInfo Method { get; internal set; }
        public DiscordEventAttribute Attribute { get; internal set; }
    }
}
