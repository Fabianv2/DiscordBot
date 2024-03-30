using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordBot.Services
{
    public class Localtime
    {
        public DateTime GetLocaltime(int unixTimeStamp, int timezone)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime localTime = unixEpoch.AddSeconds(unixTimeStamp);

            return localTime.AddSeconds(timezone);
        }
    }
}
