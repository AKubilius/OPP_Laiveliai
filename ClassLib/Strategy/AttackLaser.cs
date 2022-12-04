using ClassLib.Strategy.Template;
using ClassLib.Units;
using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy
{
    public class AttackLaser : IStrategy
    {
        public object DoAlgorithm(Weapon weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            ClientClass.ClientCode(new LaserTemplate(), weapon, form, playerName, hubConnection, bulletId);

            return true;
        }
    }
}
