using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
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
        public async Task KickUser(IGuildUser user, [Remainder] string reason)
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
        public async Task RemoveUserMessage(int amountDeletes, [Remainder] SocketUser user = null)
        {
            try
            {

                var messages = Context.Channel.GetMessagesAsync(amountDeletes + 1).FlattenAsync();

                if (user != null)
                {
                    if (amountDeletes > 0 && amountDeletes < 26)
                    {
                        foreach (IUserMessage message in await messages)
                        {
                            if (message.Author.Id == user.Id)
                            {
                                await message.DeleteAsync();
                                Console.WriteLine($"Removed message: {message}");
                                await Task.Delay(600);
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
                else
                {
                    await (Context.Channel as ITextChannel).DeleteMessagesAsync(await messages);

                    foreach (IUserMessage message in await messages)
                    {
                        Console.WriteLine($"Removed message: {message}");
                    }
                    EmbedBuilder builder = new EmbedBuilder()
                    {
                        Description = $"{amountDeletes} Nachrichten wurden durch {Context.User.Mention} gelöscht.",
                        Color = new Color(93, 64, 242)
                    };
                    await Context.Channel.SendMessageAsync("", false, builder.Build());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}