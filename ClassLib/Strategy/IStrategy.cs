using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy
{
    public interface IStrategy
    {
        object DoAlgorithm(object weapon, Form form, string playerName, HubConnection hubConnection, int bulletId);
    }
}
