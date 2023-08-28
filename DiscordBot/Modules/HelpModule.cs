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
        LevelModule _levelModule = new LevelModule();

        private string GetPrefix() => BotManager.PREFIX;

        [Command("help")]
        [Alias("Help", "Hilfe", "hilfe")]
        public async Task SendInfo([Remainder] string topic = null)
        {
            var _gameModule = new GameModule();

            if (topic == "games" || topic == "Games")
            {
                await ShowGameHelp();
            }
            else if (topic == "language" || topic == "Language")
            {
                await ShowLanguageHelp();
            }
            else if (topic == "banking" || topic == "Banking")
            {
                await ShowBankingHelp();
            }
            else if (topic == "level" || topic == "Level")
            {
                await ShowLevelHelp();
            }
            else
            {
                await ShowGeneralHelp();
            }

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Help) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Help-Thema: {topic}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        private async Task ShowGameHelp()
        {
            var builder = new EmbedBuilder()
            {
                Title = ":video_game:Game commands",
                Color = new Color(93, 64, 242)
            };
            builder.AddField(x =>
            {
                x.Name = ":scissors:Schere :fist:Stein :raised_hand:Papier";
                x.Value = $"``{GetPrefix()}SSP`` ``[Auswahl]``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":game_die:Errate Wuerfelnummer";
                x.Value = $"``{GetPrefix()}rn`` ``[Auswahl (1-6)]``";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = $"{Context.User}-Kontostand: {_bankingModule.GetKontostand(Context)} Coins";
                footer.IconUrl = Context.User.GetAvatarUrl();
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        private async Task ShowLanguageHelp()
        {
            var builder = new EmbedBuilder()
            {
                Title = ":speaking_head:Language commands",
                Color = new Color(93, 64, 242),
            };
            builder.AddField(x =>
            {
                x.Name = "Text zu Morsecode";
                x.Value = $"``{GetPrefix()}morse`` ``[Text]``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = "Morsecode zu Text";
                x.Value = $"``{GetPrefix()}remorse`` ``[Morsecode]``";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = BotManager.BotClient.CurrentUser.ToString();
                footer.IconUrl = BotManager.BotClient.CurrentUser.GetAvatarUrl();
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        private async Task ShowBankingHelp()
        {
            var builder = new EmbedBuilder()
            {
                Title = ":moneybag:Banking commands",
                Color = new Color(93, 64, 242),
            };
            builder.AddField(x =>
            {
                x.Name = ":credit_card:Kontostand";
                x.Value = $"``{GetPrefix()}kontostand``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":money_with_wings:Coins spenden";
                x.Value = $"``{GetPrefix()}donate`` ``[@(user)]`` ``[amount]``";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = $"{Context.User}-Kontostand: {_bankingModule.GetKontostand(Context)} Coins";
                footer.IconUrl = Context.User.GetAvatarUrl();
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        private async Task ShowLevelHelp()
        {
            var builder = new EmbedBuilder()
            {
                Title = "Level commands",
                Color = new Color(93, 64, 242),
            };
            builder.AddField(x =>
            {
                x.Name = "Level anzeigen";
                x.Value = $"``{GetPrefix()}level``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = "XP anzeigen";
                x.Value = $"``{GetPrefix()}xp``";
                x.IsInline = true;
            });
            builder.WithFooter(footer =>
            {
                footer.Text = $"{Context.User}-Level: {_levelModule.GetPlayerLevel(Context)}";
                footer.IconUrl = Context.User.GetAvatarUrl();
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        private async Task ShowGeneralHelp()
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
                x.Value = $"``{GetPrefix()}help`` ``games``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":speaking_head: Language";
                x.Value = $"``{GetPrefix()}help`` ``language``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":thought_balloon: Banking";
                x.Value = $"``{GetPrefix()}help`` ``banking``";
                x.IsInline = true;
            });
            builder.AddField(x =>
            {
                x.Name = ":up: Level";
                x.Value = $"``{GetPrefix()}help`` ``level``";
                x.IsInline = true;
            });
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
