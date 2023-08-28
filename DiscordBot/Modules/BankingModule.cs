using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.AccountManager;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class BankingModule : ModuleBase<SocketCommandContext>
    {
        ServerDataManager _serverDataManager = new ServerDataManager();

        #region Commands
        [Command("Kontostand")]
        [Alias("kontostand")]
        public async Task OutputKontostand()
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}, dein Kontostand: {GetKontostand(Context)} :coin:");
        }

        [Command("donate")]
        [Alias("Donate")]
        public async Task DonateCoins(SocketGuildUser userToDonateID, [Remainder] int amount)
        {
            string username = userToDonateID.Username;
            string diskriminator = userToDonateID.Discriminator;
            string userToDonate = $"{username}#{diskriminator}";
            
            ServerData serverData = _serverDataManager.LoadServerData((Context.Channel as SocketGuildChannel).Guild);
            var userToDonateKontoToUpdate = GetCorrectUser(userToDonate, username, diskriminator, serverData);
            var userDonaterKontoToUpdate = serverData.Users.Find(user => user.Username == Context.User.ToString());

            if (userToDonateKontoToUpdate != null && userDonaterKontoToUpdate != null)
            {
                if (userDonaterKontoToUpdate.Konto < amount)
                {
                    Console.WriteLine($"Der Benutzer '@{Context.User}' hat nicht genügend Coins auf seinem Konto. Kontostand: {userDonaterKontoToUpdate.Konto} Coins");
                    await Context.Channel.SendMessageAsync($"{Context.User.Mention}, du hast nicht genug Coins, um den Betrag '{amount} :coin:' zu spenden \nSpiele ein Spiel und wieder mehr Coins auf dein Konto zu bekommen.");
                }
                else
                {
                    userToDonateKontoToUpdate.Konto += Convert.ToDouble(amount);
                    userDonaterKontoToUpdate.Konto -= Convert.ToDouble(amount);

                    _serverDataManager.SaveServerData(Context.Guild.ToString(), serverData);

                    Console.WriteLine($"Das Konto von '{userToDonateKontoToUpdate}' wurde um {amount} erhöht. Neuer Kontostand: {userToDonateKontoToUpdate.Konto} Coins");
                    Console.WriteLine($"Das Konto von '{Context.User}' wurde um {amount} gesenkt. Neuer Kontostand: {userDonaterKontoToUpdate.Konto} Coins");
                    await Context.Channel.SendMessageAsync($"{userToDonateID.Mention}, der User {Context.User.Mention} hat dir {amount} :coin: gespendet!");
                }
            }
            else
            {
                Console.WriteLine($"Der Benutzer '@{userToDonateKontoToUpdate}' wurde nicht gefunden.");
            }
        }

        #endregion Commands


        #region public Methoden
        #region GetCorrectUser
        public UserData GetCorrectUser(string userToDonate, string username, string diskriminator, ServerData serverData)
        {
            if (userToDonate.Contains("#0000"))
            {
                return serverData.Users.Find(user => user.Username == username);
            }
            else
            {
                return serverData.Users.Find(user => user.Username.Contains("#" + diskriminator) == userToDonate.Contains("#" + diskriminator));
            }
        }
        #endregion GetCorrectUser

        #region GetKontostand
        public string GetKontostand(ICommandContext context)
        {
            ServerData serverData = _serverDataManager.LoadServerData((context.Channel as SocketGuildChannel).Guild);
            var getUserCurrentKonto = serverData.Users.Find(user => user.Username == context.User.ToString());

            return getUserCurrentKonto.Konto.ToString("#,0");
        }
        #endregion GetKontostand

        #region UpdateKonto
        public void UpdateKonto(ICommandContext context, int amount)
        {
            try
            {
                var serverData = _serverDataManager.LoadServerData((context.Channel as SocketGuildChannel).Guild);
                var targetServer = context.Guild;
                var userKontoToUpdate = serverData.Users.Find(user => user.Username == context.User.ToString());
                string getAmountOperator = amount.ToString().Substring(0, 1);

                if (userKontoToUpdate != null)
                {
                    if (userKontoToUpdate.Konto > 0 && getAmountOperator == "-")
                    {
                        userKontoToUpdate.Konto -= Convert.ToDouble(amount);
                    }
                    else
                    {
                        userKontoToUpdate.Konto += Convert.ToDouble(amount);
                    }

                    _serverDataManager.SaveServerData(context.Guild.ToString(), serverData);

                    Console.WriteLine($"Das Konto von '{context.User}' wurde um {amount} erhöht. Neuer Kontostand: {userKontoToUpdate.Konto}");
                }
                else
                {
                    Console.WriteLine($"Der Benutzer '{context.User}' wurde nicht gefunden.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion UpdateKonto
        #endregion public Methoden
    }
}
