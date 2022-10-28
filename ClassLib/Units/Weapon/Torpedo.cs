using ClassLib.Builder;
using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLib.Command;

namespace ClassLib.Units
{
    public class Torpedo : Weapon
    {
        public void mkBullet(Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            // this function will add the bullet to the game play
            // it is required to be called from the main class
            _bulletId = bulletId;
            _hubConnection = hubConnection;
            _playerName = playerName;
            bullet.BackColor = System.Drawing.Color.Black; // set the colour white for the bullet
            bullet.Size = new Size(10, 10); // set the size to the bullet to 5 pixel by 5 pixel
            bullet.Tag = "bullet"; // set the tag to bullet
            bullet.Left = bulletLeft; // set bullet left 
            bullet.Top = bulletTop; // set bullet right
            bullet.BringToFront(); // bring the bullet to front of other objects
            form.Controls.Add(bullet); // add the bullet to the screen
            _clientSize = form.ClientSize;

            tm.Interval = speed; // set the timer interval to speed
            tm.Tick += new EventHandler(tm_Tick); // assignment the timer with an event
            tm.Start(); // start the timer
        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }

        public async void tm_Tick(object sender, EventArgs e)
        {
            // if direction equals to left
            if (direction == "left")
            {
                bullet.Left -= speed; // move bullet towards the left of the screen
            }
            // if direction equals right
            if (direction == "right")
            {
                bullet.Left += speed; // move bullet towards the right of the screen
            }
            // if direction is up
            if (direction == "up")
            {
                bullet.Top -= speed; // move the bullet towards top of the screen
            }
            // if direction is down
            if (direction == "down")
            {
                bullet.Top += speed; // move the bullet bottom of the screen
            }
            Location location = new Location("MoveBullet", _playerName, _bulletId, bullet.Location.X, bullet.Location.Y);
            Command cmd = new Command("Location", JsonConvert.SerializeObject(location));
            await SendAsync(cmd);

            if (bullet.Left < -50 || bullet.Left > _clientSize.Width || bullet.Top < 0 || bullet.Top > _clientSize.Height)
            {
                tm.Stop(); // stop the timer
                tm.Dispose(); // dispose the timer event and component from the program
                bullet.Dispose(); // dispose the bullet
                tm = null; // nullify the timer object
                bullet = null; // nullify the bullet object
            }
        }
    }
}
