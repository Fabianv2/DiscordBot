using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.AccountManager
{
    public class UserData
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string PrevNicknames {  get; set; }
        public double Konto { get; set; }
        public int Level { get; set; }
        public double EXP { get; set; }
        public string JoinedAt {  get; set; }
    }
}
