using ClassLib.Units;
using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLib.Strategy
{
    public class AttackTorpedo : IStrategy
    {
        public object DoAlgorithm(object weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            var torpedo = weapon as Torpedo;
            torpedo.mkBullet(form, playerName, hubConnection, bulletId);
            return true;
        }
    }
}
