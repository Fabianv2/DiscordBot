using Discord.Commands;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        BankingModule _bankingModule = new BankingModule();
        Random random = new Random();

        #region Schere Stein Papier
        [Command("SSP")]
        [Alias("ssp")]
        public async Task SchereSteinPapier([Remainder] string playerSelection)
        {
            List<string> auswahlListe = new List<string>
            { "schere","stein","papier" };
            string _playerSelection = playerSelection.ToLower();
            string botSelection;

            int i = random.Next(0, 3);
            botSelection = auswahlListe[i];
            try
            {
                if (_playerSelection == botSelection)
                {
                    await Context.Channel.SendMessageAsync($"{botSelection} \nUnentschieden! \n{Context.Message.Author.Mention}, versuche es nochmal!");
                }
                else if (_playerSelection == "schere" && botSelection == "papier" ||
                         _playerSelection == "stein" && botSelection == "schere" ||
                         _playerSelection == "papier" && botSelection == "stein")
                {
                    _bankingModule.UpdateKonto(Context, 2);
                    await Context.Channel.SendMessageAsync($"{botSelection} \n{Context.Message.Author.Mention}, du hast gewonnen! (+2 :coin:) \nAktueller Kontostand: {_bankingModule.GetKontostand(Context)} :coin:");
                }
                else if (_playerSelection == "schere" && botSelection == "stein" ||
                         _playerSelection == "stein" && botSelection == "papier" ||
                         _playerSelection == "papier" && botSelection == "schere")
                {
                    _bankingModule.UpdateKonto(Context, -1);
                    await Context.Channel.SendMessageAsync($"{botSelection} \n{Context.Message.Author.Mention}, du hast verloren! (-1 :coin:) \nAktueller Kontostand: {_bankingModule.GetKontostand(Context)} :coin:");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Game: SSP) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Player: {playerSelection}");
            Console.WriteLine($"Bot: {botSelection}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
        #endregion Schere Stein Papier

        #region Errate Wuerfelnummer
        [Command("RN")]
        [Alias("rn", "Rn", "rN")]
        public async Task Wuerfeln(int guess)
        {
            int botNumber = random.Next(1, 7);
            int playerGuess = guess;

            try
            {
                if (playerGuess == botNumber)
                {
                    _bankingModule.UpdateKonto(Context, 8);
                    await Context.Channel.SendMessageAsync($"{botNumber} :game_die: \n{Context.Message.Author.Mention}, du hast die richtige Zahl erraten! (+8 :coin:) \nAktueller Kontostand: {_bankingModule.GetKontostand(Context)} :coin:");
                }
                else
                {
                    _bankingModule.UpdateKonto(Context, -1);
                    await Context.Channel.SendMessageAsync($"{botNumber} :game_die: \n{Context.Message.Author.Mention}, du hast die Zahl leider nicht erraten! (-1 :coin:) \nAktueller Kontostand: {_bankingModule.GetKontostand(Context)} :coin:");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine($"Server: {Context.Guild}");
            Console.WriteLine($"(Game: RN) {Context.User.ToString()}: {Context.Message}");
            Console.WriteLine($"Playerguess: {playerGuess}");
            Console.WriteLine($"Botgeuss: {botNumber}");
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
        #endregion Errate Wuerfelnummer
    }
}
