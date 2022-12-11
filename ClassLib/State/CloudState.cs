using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Command;

namespace ClassLib.State
{
    public class CloudState : State
    {
        public override Command CreateCommand()
        {
            MatchEvent events = new MatchEvent("MapEvent", "map3");
            Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(events));
            return cmd;
        }
    }
}
