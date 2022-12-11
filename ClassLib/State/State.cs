using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Command;

namespace ClassLib.State
{
    public class State
    {
        public virtual Command CreateCommand()
        {
            MatchEvent events = new MatchEvent("MapEvent", "map");
            Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(events));
            return cmd;
        }
    }
}
