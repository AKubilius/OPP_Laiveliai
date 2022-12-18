using ClassLib;
using Client.Properties;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ClassLib.Command;

namespace Client.Chain
{
    public class DamageChecker : ChainHandler
    {
        public HubConnection _hubConnection;
        public override async void ProcessRequest(int playerHealth, int bulletDamage, string playerName, Map map)
        {
            _hubConnection = map._hubConnection;

            if (map.player.Health.Value <= bulletDamage)
            {
                _successor.ProcessRequest(playerHealth, bulletDamage, playerName, map);
            }
            else
            {
                map.player.Health.Value -= bulletDamage;

                int xAxis = map.player.Image.Location.X - 40;
                int yAxis = map.player.Image.Location.Y - 40;

                Location playerHit = new Location("MoveParticle", playerHealth, playerName, xAxis, yAxis);
                Command cmd = new Command("Location", JsonConvert.SerializeObject(playerHit));
                await SendAsync(cmd);
            }
        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }
    }
}
