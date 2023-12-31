﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.AccountManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("Ban")]
        [Alias("ban")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Du hast nicht die Berechtigung, User zu bannen!")]
        public async Task BanUser(IGuildUser user, [Remainder] string reason = null)
        {
            try
            {
                if (user == null)
                {
                    await Context.Channel.SendMessageAsync("Gib einen User an!");
                }

                if (reason == null)
                {
                    reason = "Es wurde kein Ban-Grund angegeben.";
                }

                await Context.Guild.AddBanAsync(user, 1, reason);

                EmbedBuilder builder = new EmbedBuilder()
                {
                    Description = $":white_check_mark: {user.Mention} wurde von {Context.User} gebannt. \n**Grund** {reason}",
                    Color = new Color(93, 64, 242)
                };
                await Context.Channel.SendMessageAsync("", false, builder.Build());
                Console.WriteLine($"Der Spieler '{user}', wurde von '{Context.User}' gebannt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Command("Unban")]
        [Alias("unban")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Du hast nicht die Berechtigung, den Ban eines Users rückgängig zu machen!")]
        public async Task UnbanUser(IGuildUser user)
        {
            try
            {
                if (user == null)
                {
                    await Context.Channel.SendMessageAsync("Gib einen User an!");
                }

                await Context.Guild.RemoveBanAsync(user);

                EmbedBuilder builder = new EmbedBuilder()
                {
                    Description = $":white_check_mark: {user.Mention} wurde von {Context.User} entbannt",
                    Color = new Color(93, 64, 242)
                };
                await Context.Channel.SendMessageAsync("", false, builder.Build());
                Console.WriteLine($"Der Spieler '{user}', wurde von '{Context.User}' entbannt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Command("Kick")]
        [Alias("kick")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Du hast nicht die Berechtigung, User zu kicken!")]
        public async Task KickUser(IGuildUser user, [Remainder]string reason)
        {
            try
            {
                if (user == null)
                {
                    await Context.Channel.SendMessageAsync("Gib einen User an!");
                }

                await user.KickAsync(reason);

                EmbedBuilder builder = new EmbedBuilder()
                {
                    Description = $":white_check_mark: {user.Mention} wurde von {Context.User} gekickt",
                    Color = new Color(93, 64, 242)
                };
                await Context.Channel.SendMessageAsync("", false, builder.Build());
                Console.WriteLine($"Der Spieler '{user}', wurde von '{Context.User}' gekickt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Command("rm")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "Du hast nicht die Berechtigung, Nachrichten anderer User zu löschen!")]
        public async Task RemoveUserMessage(SocketUser user, int amountDeletes)
        {
            try
            {
                if (amountDeletes > 0 && amountDeletes <= 25)
                {
                    var messages = Context.Channel.GetMessagesAsync(amountDeletes).FlattenAsync();

                    foreach (IUserMessage message in await messages)
                    {
                        if (message.Author.Id == user.Id)
                        {
                            await message.DeleteAsync();
                            Console.WriteLine($"Removed message: {message}");
                        }
                    }
                    EmbedBuilder builder = new EmbedBuilder()
                    {
                        Description = $"{amountDeletes} Nachrichten von {user.Mention}, wurden durch {Context.User.Mention} gelöscht.",
                        Color = new Color(93, 64, 242)
                    };
                    await Context.Channel.SendMessageAsync("", false, builder.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{Context.User.Mention}, gib eine Zahl von 1-25 an!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}