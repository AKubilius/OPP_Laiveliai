using ClassLib.Builder;
using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ClassLib.Command;
using Timer = System.Windows.Forms.Timer;

namespace ClassLib.Strategy.Template
{
    public abstract class TemplateAbstractClass
    {
        public string direction; // creating a public string called direction
        public int speed = 20; // creating a integer called speed and assigning a value of 20
        public PictureBox bullet = new PictureBox(); // create a picture box 
        public Timer tm = new Timer(); // create a new timer called tm. 

        public int bulletLeft; // create a new public integer
        public int bulletTop; // create a new public integer

        public Form _form;
        public HubConnection _hubConnection;
        public string _playerName;
        public string _bulletType;
        public int _bulletId;

        public int _bulletWidth;
        public int _bulletHeight;
        public int _bulletSpeed;
        public int _bulletDamage;
        public Color _bulletColor;
        public Size _clientSize;

        public void TemplateMethod()
        {
            SetBulletParams();
            ShootBullet();
        }

        public abstract void SetBulletParams();
        public void ShootBullet()
        {
            // this function will add the bullet to the game play
            // it is required to be called from the main class
            //_bulletId = bulletId;
            //_hubConnection = hubConnection;
            //_playerName = playerName;
            bullet.BackColor = _bulletColor; // set the colour white for the bullet
            if (direction == "left" || direction == "right")
            {
                bullet.Size = new Size(_bulletWidth, _bulletHeight); // set the size to the bullet to 5 pixel by 5 pixel
            }
            else
            {
                bullet.Size = new Size(_bulletHeight, _bulletWidth); // set the size to the bullet to 5 pixel by 5 pixel
            }
            bullet.Tag = "bullet"; // set the tag to bullet
            bullet.Left = bulletLeft; // set bullet left 
            bullet.Top = bulletTop; // set bullet right
            bullet.BringToFront(); // bring the bullet to front of other objects
            _form.Controls.Add(bullet); // add the bullet to the screen
            _clientSize = _form.ClientSize;
            //form.Controls.Add(bullet); // add the bullet to the screen
            //_clientSize = form.ClientSize;

            tm.Interval = _bulletSpeed; // set the timer interval to speed
            tm.Tick += new EventHandler(tm_Tick); // assignment the timer with an event
            tm.Start(); // start the timer
        }

        public void SetConnection(Weapon weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            bulletTop = weapon.bulletTop;
            bulletLeft = weapon.bulletLeft;
            direction = weapon.direction;
            _form = form;
            _playerName = playerName;
            _hubConnection = hubConnection;
            _bulletId = bulletId;
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
            Location location = new Location("MoveBullet", _playerName, _bulletDamage, _bulletType, _bulletId, bullet.Size.Width, bullet.Size.Height, bullet.Location.X, bullet.Location.Y);
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
