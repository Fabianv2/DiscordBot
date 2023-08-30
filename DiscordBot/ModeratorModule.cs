using Discord;
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
                    Description = $":white_check_mark: {user.Mention} wurde gebannt \n**Grund** {reason}",
                    Color = new Color(93, 64, 242)
                };
                builder.WithFooter(footer =>
                {
                    footer.Text = "User Ban Log";
                    footer.WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
                await Context.Channel.SendMessageAsync("", false, builder.Build());
                Console.WriteLine($"Der Spieler '{user}', wurde von '{Context.User}' gebannt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}