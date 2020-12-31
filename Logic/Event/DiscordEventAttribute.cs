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

            Task OnEventWithArg(object s)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (listener.Method.IsStatic)
                            await (Task)listener.Method.Invoke(null, new object[] { bot, client, s });
                        else
                        {
                            var instance = Activator.CreateInstance(listener.Type);
                            await (Task)listener.Method.Invoke(instance, new object[] { bot, client, s });
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

            Task OnEventWithTwoArgs(object s, object e)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (listener.Method.IsStatic)
                            await (Task)listener.Method.Invoke(null, new object[] { bot, client, s, e });
                        else
                        {
                            var instance = Activator.CreateInstance(listener.Type);
                            await (Task)listener.Method.Invoke(instance, new object[] { bot, client, s, e });
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

            Task OnEventWithThreeArgs(object s, object e1, object e2)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (listener.Method.IsStatic)
                            await (Task)listener.Method.Invoke(null, new object[] { bot, client, s, e1, e2 });
                        else
                        {
                            var instance = Activator.CreateInstance(listener.Type);
                            await (Task)listener.Method.Invoke(instance, new object[] { bot, client, s, e1, e2 });
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
                        => OnEventWithTwoArgs(b, a);
                    break;
                case EventType.Ready:
                    client.Ready += OnEvent;
                    break;
                case EventType.CurrentUserUpdated:
                    client.CurrentUserUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.ChannelCreated:
                    client.ChannelCreated += OnEventWithArg;
                    break;
                case EventType.ChannelUpdated:
                    client.ChannelUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.ChannelDestroyed:
                    client.ChannelDestroyed += OnEventWithArg;
                    break;
                case EventType.GuildAvailable:
                    client.GuildAvailable += OnEventWithArg;
                    break;
                case EventType.GuildMembersDownloaded:
                    client.GuildMembersDownloaded += OnEventWithArg;
                    break;
                case EventType.GuildMemberUpdated:
                    client.GuildMemberUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.GuildUnavailable:
                    client.GuildUnavailable += OnEventWithArg;
                    break;
                case EventType.GuildUpdated:
                    client.GuildUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.InviteCreated:
                    client.InviteCreated += OnEventWithArg;
                    break;
                case EventType.InviteDeleted:
                    client.InviteDeleted += OnEventWithTwoArgs;
                    break;
                case EventType.JoinedGuild:
                    client.JoinedGuild += OnEventWithArg;
                    break;
                case EventType.LeftGuild:
                    client.LeftGuild += OnEventWithArg;
                    break;
                case EventType.MessageDeleted:
                    client.MessageDeleted += (m, c)
                        => OnEventWithTwoArgs(m, c);
                    break;
                case EventType.MessageReceived:
                    client.MessageReceived += OnEventWithArg;
                    break;
                case EventType.MessageUpdated:
                    client.MessageUpdated += (b, a, c)
                        => OnEventWithThreeArgs(b, a, c);
                    break;
                case EventType.ReactionAdded:
                    client.ReactionAdded += (m, c, r)
                        => OnEventWithThreeArgs(m, c, r);
                    break;
                case EventType.ReactionRemoved:
                    client.ReactionRemoved += (m, c, r)
                        => OnEventWithThreeArgs(m, c, r);
                    break;
                case EventType.ReactionsCleared:
                    client.ReactionsCleared += (m, c)
                        => OnEventWithTwoArgs(m, c);
                    break;
                case EventType.ReactionsRemovedForEmote:
                    client.ReactionsRemovedForEmote += (m, c, e)
                        => OnEventWithThreeArgs(m, c, e);
                    break;
                case EventType.RecipientAdded:
                    client.RecipientAdded += OnEventWithArg;
                    break;
                case EventType.RecipientRemoved:
                    client.RecipientRemoved += OnEventWithArg;
                    break;
                case EventType.RoleCreated:
                    client.RoleCreated += OnEventWithArg;
                    break;
                case EventType.RoleDeleted:
                    client.RoleDeleted += OnEventWithArg;
                    break;
                case EventType.RoleUpdated:
                    client.RoleUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.UserBanned:
                    client.UserBanned += OnEventWithTwoArgs;
                    break;
                case EventType.UserIsTyping:
                    client.UserIsTyping += OnEventWithTwoArgs;
                    break;
                case EventType.UserJoined:
                    client.UserJoined += OnEventWithArg;
                    break;
                case EventType.UserLeft:
                    client.UserLeft += OnEventWithArg;
                    break;
                case EventType.UserUnbanned:
                    client.UserUnbanned += OnEventWithTwoArgs;
                    break;
                case EventType.UserUpdated:
                    client.UserUpdated += OnEventWithTwoArgs;
                    break;
                case EventType.UserVoiceStateUpdated:
                    client.UserVoiceStateUpdated += (u, b, a)
                        => OnEventWithThreeArgs(u, b, a);
                    break;
                case EventType.VoiceServerUpdated:
                    client.VoiceServerUpdated += OnEventWithArg;
                    break;
                case EventType.Log:
                    client.Log += (m)
                        => OnEventWithArg(m);
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
