using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.AccountManager
{
    public class TimerManager
    {
        BotManager _botManager = new BotManager();
        private DiscordSocketClient _botClient;
        private Timer _timer;

        public TimerManager(DiscordSocketClient botClient)
        {
            _botClient = botClient;
        }

        public void Start()
        {
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        }

        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, 0);
        }

        private async void TimerCallback(object state)
        {
            await UpdateUserdata();
        }

        private async Task UpdateUserdata()
        {
            await _botManager.UpdateUser(_botClient);
            Console.WriteLine($"{DateTime.Now}: Userdaten wurden geupdated");
        }
    }
}