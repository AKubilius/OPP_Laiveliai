using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy.Template
{
    public class ClientClass
    {
        public static void ClientCode(TemplateAbstractClass abstractClass, Weapon weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            abstractClass.SetConnection(weapon, form, playerName, hubConnection, bulletId);
            abstractClass.TemplateMethod();
        }
    }
}
