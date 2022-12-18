using ClassLib.State;
using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Command;
using ClassLib;
using Newtonsoft.Json;
using System.Net.Security;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClassLib.Visitor
{
    public class ParticleVisitor : IVisitor
    {
        public void Visit(Element element)
        {
            Player player = element as Player;

            Location location = new Location();
            location.Response = "UpdateParticleLocationn";
            Command answer = new Command("Location", JsonConvert.SerializeObject(location));
            SendAsyncMessage(answer);
        }

        public async void SendAsyncMessage(Command location)
        {

        }
    }
}
