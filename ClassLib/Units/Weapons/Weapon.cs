using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Drawing;
using System.Windows.Forms;
using static ClassLib.Command;
using Timer = System.Windows.Forms.Timer;

namespace ClassLib.Units.Bullet
{
    public class Weapon
    {
        public string direction; // creating a public string called direction
        public int speed = 20; // creating a integer called speed and assigning a value of 20
        public PictureBox bullet = new PictureBox(); // create a picture box 
        public Timer tm = new Timer(); // create a new timer called tm. 

        public int bulletLeft; // create a new public integer
        public int bulletTop; // create a new public integer

        public HubConnection _hubConnection;
        public string _playerName;
        public int _bulletId;

        public Size _clientSize;

        public Weapon()
        {

        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }
    }
}
