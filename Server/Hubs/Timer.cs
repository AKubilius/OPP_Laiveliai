using ClassLib;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class Timer:Hub
    {



        public async Task StartTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += async (sender, e) => await OnTimedEvent();
            aTimer.Interval = 20000;
            aTimer.Enabled = true;
            Console.WriteLine("bbbb");

        }
        private async Task OnTimedEvent()
        {

            Command command = new Command("ChangeMap", "Red");
            await Clients.All.SendAsync("Message", command);
            Console.WriteLine("aaa");
            //await SendAllAsync(command);
        }
    }
}
