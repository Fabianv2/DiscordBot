using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class TestCommandsModule : ModuleBase<SocketCommandContext>
    {
        GameModule _gameModule = new GameModule();

        [Command("hallo")]
        [Alias("hi", "Hallo")]
        public async Task Hallo([Remainder] string user)
        {
            await Context.Channel.SendMessageAsync("Hallöle " + user);
        }

        [Command("Test")]
        public async Task Rueckmeldung()
        {
            await Context.Channel.SendMessageAsync($"Hallo {Context.Message.Author.Mention}");
        }

        [Command("calc")]
        [Alias("Calc")]
        public async Task Calc(int i1, int i2)
        {
            int ergebnis = i1 + i2;
            await Context.Channel.SendMessageAsync(ergebnis.ToString());
        }

        [Command("random")]
        [Alias("Random")]
        public async Task RandomNumber(int number1, int number2)
        {
            Random random = new Random();
            int generatedNumber = random.Next(number1, number2 + 1);
            await Context.Channel.SendMessageAsync($"{Context.User.Mention} \nZahl: {generatedNumber}");
        }

        [Command("Avatar")]
        [Alias("avatar")]
        public async Task GetAvatar()
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(93, 64, 242),
                ImageUrl = Context.User.GetAvatarUrl()
            };
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
