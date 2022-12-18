using ClassLib;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Command;

namespace Client.Chain
{
    public class PlayerKicker : ChainHandler
    {
        public HubConnection _hubConnection;
        public override async void ProcessRequest(int playerHealth, int bulletDamage, string playerName, Map map)
        {
            // Kick player
            map.Close();
        }
    }
}
