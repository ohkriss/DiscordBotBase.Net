using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using System.Diagnostics;

namespace DiscordBotBase.Logic.Event
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DiscordEventAttribute : Attribute
    {
        public EventType Target { get; }
        public DiscordEventAttribute(EventType targetType)
            => Target = targetType;

        public void Register(BotShard bot, DiscordSocketClient client, ListenerMethod listener)
        {
            Task OnEvent()
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (listener.Method.IsStatic)
                            await (Task)listener.Method.Invoke(null, new object[] { bot, client });
                        else
                        {
                            var instance = Activator.CreateInstance(listener.Type);
                            await (Task)listener.Method.Invoke(instance, new object[] { bot, client });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[EventListener] Uncaught exception in thread >> {ex.Message} >> {ex.StackTrace}");
                        Debug.WriteLine($"[EventListener] Uncaught exception in thread >> {ex.Message} >> {ex.StackTrace}");
                    }
                });

                return Task.CompletedTask;
            }

            Task OnEventWithArgs(params object[] args)
            {
                _ = Task.Run(async () =>
                {
                    var parameters = new object[] { bot, client };
                    foreach (var a in args)
                        parameters.Append(a);
                    try
                    {
                        if (listener.Method.IsStatic)
                            await (Task)listener.Method.Invoke(null, parameters);
                        else
                        {
                            var instance = Activator.CreateInstance(listener.Type);
                            await (Task)listener.Method.Invoke(instance, parameters);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[EventListener] Uncaught exception in thread >> {ex.Message} >> {ex.StackTrace}");
                        Debug.WriteLine($"[EventListener] Uncaught exception in thread >> {ex.Message} >> {ex.StackTrace}");
                    }
                });

                return Task.CompletedTask;
            }           

            switch (Target)
            {
                case EventType.Connected:
                    client.Connected += OnEvent;
                    break;
                case EventType.Disconnected:
                    client.Connected += OnEvent;
                    break;
                case EventType.LatencyUpdated:
                    client.LatencyUpdated += (b, a)
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.Ready:
                    client.Ready += OnEvent;
                    break;
                case EventType.CurrentUserUpdated:
                    client.CurrentUserUpdated += (b, a) 
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.ChannelCreated:
                    client.ChannelCreated += (c) 
                        => OnEventWithArgs(c);
                    break;
                case EventType.ChannelUpdated:
                    client.ChannelUpdated += (b, a)
                        => OnEventWithArgs(b, a); 
                    break;
                case EventType.ChannelDestroyed:
                    client.ChannelDestroyed += (c)
                        => OnEventWithArgs(c); 
                    break;
                case EventType.GuildAvailable:
                    client.GuildAvailable += (g)
                        => OnEventWithArgs(g); 
                    break;
                case EventType.GuildMembersDownloaded:
                    client.GuildMembersDownloaded += (g)
                        => OnEventWithArgs(g); 
                    break;
                case EventType.GuildMemberUpdated:
                    client.GuildMemberUpdated += (b, a)
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.GuildUnavailable:
                    client.GuildUnavailable += (g)
                        => OnEventWithArgs(g);
                    break;
                case EventType.GuildUpdated:
                    client.GuildUpdated += (b, a)
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.InviteCreated:
                    client.InviteCreated += (i)
                        => OnEventWithArgs(i);
                    break;
                case EventType.InviteDeleted:
                    client.InviteDeleted += (c, i) 
                        => OnEventWithArgs(c, i);
                    break;
                case EventType.JoinedGuild:
                    client.JoinedGuild += (g)
                        => OnEventWithArgs(g);
                    break;
                case EventType.LeftGuild:
                    client.LeftGuild += (g)
                        => OnEventWithArgs(g);
                    break;
                case EventType.MessageDeleted:
                    client.MessageDeleted += (m, c)
                        => OnEventWithArgs(m, c);
                    break;
                case EventType.MessageReceived:
                    client.MessageReceived += (m)
                        => OnEventWithArgs(m);
                    break;
                case EventType.MessageUpdated:
                    client.MessageUpdated += (b, a, c)
                        => OnEventWithArgs(b, a, c);
                    break;
                case EventType.ReactionAdded:
                    client.ReactionAdded += (m, c, r)
                        => OnEventWithArgs(m, c, r);
                    break;
                case EventType.ReactionRemoved:
                    client.ReactionRemoved += (m, c, r)
                        => OnEventWithArgs(m, c, r);
                    break;
                case EventType.ReactionsCleared:
                    client.ReactionsCleared += (m, c)
                        => OnEventWithArgs(m, c);
                    break;
                case EventType.ReactionsRemovedForEmote:
                    client.ReactionsRemovedForEmote += (m, c, e)
                        => OnEventWithArgs(m, c, e);
                    break;
                case EventType.RecipientAdded:
                    client.RecipientAdded += (u)
                        => OnEventWithArgs(u);
                    break;
                case EventType.RecipientRemoved:
                    client.RecipientRemoved += (u)
                        => OnEventWithArgs(u);
                    break;
                case EventType.RoleCreated:
                    client.RoleCreated += (r)
                        => OnEventWithArgs(r);
                    break;
                case EventType.RoleDeleted:
                    client.RoleDeleted += (r)
                        => OnEventWithArgs(r);
                    break;
                case EventType.RoleUpdated:
                    client.RoleUpdated += (b, a)
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.UserBanned:
                    client.UserBanned += (u, g) 
                        => OnEventWithArgs(u, g);
                    break;
                case EventType.UserIsTyping:
                    client.UserIsTyping += (u, c)
                        => OnEventWithArgs(u, c);
                    break;
                case EventType.UserJoined:
                    client.UserJoined += (u)
                        => OnEventWithArgs(u);
                    break;
                case EventType.UserLeft:
                    client.UserLeft += (u)
                        => OnEventWithArgs(u);
                    break;
                case EventType.UserUnbanned:
                    client.UserUnbanned += (u, g)
                        => OnEventWithArgs(u, g);
                    break;
                case EventType.UserUpdated:
                    client.UserUpdated += (b, a)
                        => OnEventWithArgs(b, a);
                    break;
                case EventType.UserVoiceStateUpdated:
                    client.UserVoiceStateUpdated += (u, b, a)
                        => OnEventWithArgs(u, b, a);
                    break;
                case EventType.VoiceServerUpdated:
                    client.VoiceServerUpdated += (v)
                        => OnEventWithArgs(v);
                    break;
                case EventType.Log:
                    client.Log += (m)
                        => OnEventWithArgs(m);
                    break;
                case EventType.LoggedIn:
                    client.LoggedIn += OnEvent;
                    break;
                case EventType.LoggedOut:
                    client.LoggedOut += OnEvent;
                    break;
            }
        }
    }
}
