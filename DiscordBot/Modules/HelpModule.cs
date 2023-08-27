using Discord.Commands;
using Discord;
using System;
using System.Threading.Tasks;
using DiscordBot.Modules;

namespace DiscordBot
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        BankingModule _bankingModule = new BankingModule();

        string _PREFIX = BotManager.PREFIX;

        [Command("help")]
        [Alias("Help", "Hilfe", "hilfe")]
        public async Task SendInfo([Remainder] string topic = null)
        {
            var _gameModule = new GameModule();

            if (topic == "games")
            {
                var builder = new EmbedBuilder()
                {
                    Title = ":video_game:Game commands",
                    Color = new Color(93, 64, 242)
                };
                builder.AddField(x =>
                {
                    x.Name = ":scissors:Schere :fist:Stein :raised_hand:Papier";
                    x.Value = $"``{_PREFIX}SSP`` ``[Auswahl]``";
                    x.IsInline = true;
                });
                builder.AddField(x =>
                {
                    x.Name = ":game_die:Errate Wuerfelnummer";
                    x.Value = $"``{_PREFIX}rn`` ``[Auswahl (1-6)]``";
                    x.IsInline = true;
                });
                builder.WithFooter(footer =>
                {
                    footer.Text = $"{Context.User}-Kontostand: {_bankingModule.GetKontostand(Context)} Coins";
                    footer.IconUrl = Context.User.GetAvatarUrl();
                });
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else if (topic == "language")
            {
                var builder = new EmbedBuilder()
                {
                    Title = ":speaking_head:Language commands",
                    Color = new Color(93, 64, 242),
                };
                builder.AddField(x =>
                {
                    x.Name = "Text zu Morsecode";
                    x.Value = $"``{_PREFIX}morse`` ``[Text]``";
                    x.IsInline = true;
                });
                builder.AddField(x =>
                {
                    x.Name = "Morsecode zu Text";
                    x.Value = $"``{_PREFIX}remorse`` ``[Morsecode]``";
                    x.IsInline = true;
                });
                builder.WithFooter(footer =>
                {
                    footer.Text = BotManager.BotClient.CurrentUser.ToString();
                    footer.IconUrl = BotManager.BotClient.CurrentUser.GetAvatarUrl();
                });
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else if (topic == "banking")
            {
                var builder = new EmbedBuilder()
                {
                    Title = ":moneybag:banking commands",
                    Color = new Color(93, 64, 242),
                };
                builder.AddField(x =>
                {
                    x.Name = ":credit_card:Kontostand";
                    x.Value = $"``{_PREFIX}kontostand``";
                    x.IsInline = true;
                });
                builder.AddField(x =>
                {
                    x.Name = ":money_with_wings:Coins spenden";
                    x.Value = $"``{_PREFIX}donate`` ``[@(user)]``";
                    x.IsInline = true;
                });
                builder.WithFooter(footer =>
                {
                    footer.Text = $"{Context.User}-Kontostand: {_bankingModule.GetKontostand(Context)} Coins";
                    footer.IconUrl = Context.User.GetAvatarUrl();
                });
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                var builder = new EmbedBuilder()
                {
                    Title = "Fabl-Bot Commands menu",
                    Color = new Color(93, 64, 242),
                    Description = "Commands",
                    ThumbnailUrl = BotManager.BotClient.CurrentUser.GetAvatarUrl(),
                };
                builder.AddField(x =>
                {
                    x.Name = ":video_game: Games";
                    x.Value = $"``{_PREFIX}help`` ``games``";
                    x.IsInline = true;
                });
                builder.AddField(x =>
                {
                    x.Name = ":speaking_head: Language";
                    x.Value = $"``{_PREFIX}help`` ``language``";
                    x.IsInline = true;
                });
                builder.AddField(x =>
                {
                    x.Name = ":thought_balloon: banking";
                    x.Value = $"``{_PREFIX}help`` ``banking``";
                    x.IsInline = true;
                });
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Help) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Help-Thema: {topic}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
    }
}
