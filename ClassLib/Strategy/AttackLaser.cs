using ClassLib.Units;
using ClassLib.Units.Weapons;
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
        public object DoAlgorithm(object weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            var laser = weapon as Laser;
            laser.mkBullet(form, playerName, hubConnection, bulletId);
            return true;
        }
    }
}
