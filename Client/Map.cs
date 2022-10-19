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
        string _playerName;
        Ship player;
        Game _mainMenu;
        bool _isForcedToLeave;
        Dictionary<int, PictureBox> _bullets;

        public Map(HubConnection hubConnection, string playerName, int startingId, int randomY, Game mainMenu)
        {
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _isForcedToLeave = false;
            _mainMenu = mainMenu;
            _bullets = new Dictionary<int, PictureBox>();

            InitializeComponent();

            NewShip(playerName);

            player = ships[playerName];
            player.Label.Text = playerName;

            player.Image.Location = new Point(100 + (startingId * 500), randomY);
            player.Health.Location = new Point(player.Image.Location.X + 6, player.Image.Location.Y + 50);
            player.Label.Location = new Point(player.Image.Location.X, player.Image.Location.Y - 50);
            player.Image.Image = Properties.Resources.shipRight;


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
                }
            });
        }

        private Ship NewShip(string playerName)
        {
            Ship ship = new Ship(playerName);
            ships[playerName] = ship;
            ((System.ComponentModel.ISupportInitialize)(ship.Image)).BeginInit();
            this.Controls.Add(ship.Image);
            this.Controls.Add(ship.Label);
            this.Controls.Add(ship.Health);
            ((System.ComponentModel.ISupportInitialize)(ship.Image)).EndInit();
            return ship;
        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }

        public void UpdateLocation(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "UpdateLocation":
                    Ship opponent = null;
                    if (!ships.ContainsKey(location.ShipName))
                    {
                        opponent = NewShip(location.ShipName);
                    } else
                    {
                        opponent = ships[location.ShipName];
                    }
                    opponent.Image.Image = Properties.Resources.shipRight;
                    opponent.Image.Location = new Point(location.XAxis, location.YAxis);
                    opponent.Label.Location = new Point(location.XAxis, location.YAxis - 50);
                    opponent.Health.Location = new Point(location.XAxis + 6, location.YAxis + 50);
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

                case "UpdateBulletLocation":
                    if (!_bullets.ContainsKey(location.BulletID))
                    {
                        PictureBox bullet = new PictureBox();
                        bullet.BackColor = System.Drawing.Color.Red; // set the colour white for the bullet
                        bullet.Size = new Size(5, 5); // set the size to the bullet to 5 pixel by 5 pixel
                        bullet.Tag = "bullet"; // set the tag to bullet
                        bullet.BringToFront(); // bring the bullet to front of other objects
                        this.Controls.Add(bullet); // add the bullet to the screen
                        _bullets[location.BulletID] = bullet;
                    }
                    PictureBox currentBullet = _bullets[location.BulletID];
                    currentBullet.Location = new Point(location.XAxis, location.YAxis);

                    if (currentBullet.Bounds.IntersectsWith(player.Image.Bounds))
                    {
                        if (player.Health.Value > 1)
                            player.Health.Value -= 5;
                        this.Controls.Remove(currentBullet);
                    }

                    break;
            }
        }

        bool moveRight, moveLeft, moveUp, moveDown;
        int speed = 8;
        string facing = "up";

        private async void moveTimerEvent(object sender, EventArgs e)
        {
            if (moveLeft && player.Image.Left > 0)
            {
                player.Image.Left -= speed;
                player.Label.Left -= speed;
                player.Health.Left -= speed;
            }

            if (moveRight && player.Image.Right < ClientSize.Width)
            {
                player.Image.Left += speed;
                player.Label.Left += speed;
                player.Health.Left += speed;
            }

            if (moveUp && player.Image.Top > 0)
            {
                player.Image.Top -= speed;
                player.Label.Top -= speed;
                player.Health.Top -= speed;
            }
            if (moveDown && player.Image.Top < ClientSize.Height)
            {
                player.Image.Top += speed;
                player.Label.Top += speed;
                player.Health.Top += speed;
            }

            Location location = new Location("MovePlayer", _playerName, facing, player.Image.Location.X, player.Image.Location.Y);

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
            shoot.mkBullet(this, _playerName, _hubConnection, DateTime.UtcNow.GetHashCode()); // run the function mkbullet from the bullet class. 

        }

        private async void Map_Closing(object sender, CancelEventArgs e)
        {
            MatchEvents match = new MatchEvents("LeftMatch");
            Command cmd = new Command("MatchEvents", JsonConvert.SerializeObject(match));
            await SendAsync(cmd);

            _mainMenu.Visible = true;
        }
    }
}