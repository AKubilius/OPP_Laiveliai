using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using ClassLib.Units.Ship;
using ClassLib.Units.Bullet;
using ClassLib;
using static ClassLib.Command;
using Newtonsoft.Json;

namespace Client
{
    public partial class Map : Form
    {
        HubConnection _hubConnection;
        int _matchId;
        string _playerName;
        Ship player;
        Ship opponent;
        Game _mainMenu;
        bool _isForcedToLeave;
        Dictionary<int, PictureBox> _bullets;

        public Map(HubConnection hubConnection, int matchId, string playerName, int startingId, int randomY, Game mainMenu)
        {
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _isForcedToLeave = false;
            _mainMenu = mainMenu;
            _bullets = new Dictionary<int, PictureBox>();

            InitializeComponent(2);

            player = ships[0];
            player.Label.Text = playerName;

            //background.Location = new Point(startingId * randomY, 100);
            //player.Image.Location = new Point(100 + (startingId * 500), randomY);

            player.Image.Location = new Point(350, 350);
            player.Label.Location = new Point(player.Image.Location.X, player.Image.Location.Y - 50);

            player.Image.Image = Properties.Resources.shipRight;

            this._matchId = matchId;
            this._playerName = playerName;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;


            _hubConnection = hubConnection;


            _hubConnection.On<Command>("Message", (cmd) =>
            {
                switch (cmd.Name)
                {
                    case "Location":
                        UpdateLocation(cmd);
                        break;
                    case "BulletLocation":
                        UpdateBulletLocation(cmd);
                        break;
                    case "MatchEvents":
                        MatchEvents(cmd);
                        break;
                }
            });
        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }

        public void UpdateBulletLocation(Command cmd)
        {
            BulletLocation bulletLocation = JsonConvert.DeserializeObject<BulletLocation>(cmd.Content);

            if (!_bullets.ContainsKey(bulletLocation.BulletID))
            {
                PictureBox bullet = new PictureBox();
                bullet.BackColor = System.Drawing.Color.Red; // set the colour white for the bullet
                bullet.Size = new Size(5, 5); // set the size to the bullet to 5 pixel by 5 pixel
                bullet.Tag = "bullet"; // set the tag to bullet
                bullet.BringToFront(); // bring the bullet to front of other objects
                this.Controls.Add(bullet); // add the bullet to the screen
                _bullets[bulletLocation.BulletID] = bullet;
            }
            PictureBox currentBullet = _bullets[bulletLocation.BulletID];
            currentBullet.Location = new Point(bulletLocation.XAxis, bulletLocation.YAxis);
        }

        public void UpdateLocation(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "UpdateLocation":
                    opponent = ships[1];
                    opponent.Image.Image = Properties.Resources.shipRight;
                    opponent.Image.Location = new Point(location.XAxis, location.YAxis);
                    opponent.Label.Location = new Point(location.XAxis, location.YAxis - 50);
                    opponent.Label.Text = location.ShipName;
                    switch (location.Facing)
                    {
                        case "right":
                            opponent.Image.Image = Properties.Resources.shipRight;
                            break;
                        case "left":
                            opponent.Image.Image = Properties.Resources.shipLeft;
                            break;
                        case "up":
                            opponent.Image.Image = Properties.Resources.shipUp;
                            break;
                        case "down":
                            opponent.Image.Image = Properties.Resources.shipDown;
                            break;
                        default:
                            break;
                    }
                    opponent.Image.Image = location.Facing.Equals("right") ? Properties.Resources.shipRight : Properties.Resources.shipLeft;
                    break;
            }
        }
        public void MatchEvents(Command cmd)
        {
            switch (cmd.Name)
            {
                case "LeaveMatch":
                    _isForcedToLeave = true;
                    this.Close();
                    break;
            }

        }

        bool moveRight, moveLeft, moveUp, moveDown;
        int speed = 8;
        string facing = "up";
        int backSpeed = 8;
        int X = 350;
        int Y = 350;


        private async void moveTimerEvent(object sender, EventArgs e)
        {
            //if (moveLeft && player.Image.Left > 0)
            //{
            //    player.Image.Left -= speed;
            //    player.Label.Left -= speed;
            //}

            //if (moveRight && player.Image.Right < 700)
            //{
            //    player.Image.Left += speed;
            //    player.Label.Left += speed;
            //}

            //if (moveUp && player.Image.Top > 0)
            //{
            //    player.Image.Top -= speed;
            //    player.Label.Top -= speed;
            //}
            //if (moveDown && player.Image.Top < 700)
            //{
            //    player.Image.Top += speed;
            //    player.Label.Top += speed;
            //}

            if (moveLeft)
            {
                background.Left += backSpeed;
                X -= backSpeed;
            }

            if (moveRight)
            {
                background.Left -= backSpeed;
                X += backSpeed;
            }
            if (moveUp)
            {
                background.Top += backSpeed;
                Y -= backSpeed;
            }

            if (moveDown)
            {
                background.Top -= backSpeed;
                Y += backSpeed;
            }


            // Location location = new Location("MovePlayer", _matchId, _playerName, facing, player.Image.Location.X, player.Image.Location.Y);
            Location location = new Location("MovePlayer", _matchId, _playerName, facing, X, Y);
            
            Command cmd = new Command("Location", JsonConvert.SerializeObject(location));
            await SendAsync(cmd);

        }
        private async void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                facing = "left";
                player.Image.Image = Properties.Resources.shipLeft;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                facing = "right";
                player.Image.Image = Properties.Resources.shipRight;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = true;
                facing = "down";
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
                facing = "up";
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                shoot(facing);
            }

        }
        private void shoot(string direct)
        {
            // this is the function thats makes the new bullets in this game

            Bullet shoot = new Bullet(); // create a new instance of the bullet class
            shoot.direction = direct; // assignment the direction to the bullet
            shoot.bulletLeft = player.Image.Left + (player.Image.Width / 2); // place the bullet to left half of the player
            shoot.bulletTop = player.Image.Top + (player.Image.Height / 2); // place the bullet on top half of the player
            shoot.mkBullet(this, _matchId, _playerName, _hubConnection, DateTime.UtcNow.GetHashCode()); // run the function mkbullet from the bullet class. 

        }

        private async void Map_Closing(object sender, CancelEventArgs e)
        {
            if (!_isForcedToLeave)
            {
                MatchEvents match = new MatchEvents("LeftMatch", _matchId);
                Command cmd = new Command("MatchEvents", JsonConvert.SerializeObject(match));
                await SendAsync(cmd);
            }

            _mainMenu.Visible = true;
        }
    }
}