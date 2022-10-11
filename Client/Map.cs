using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using ClassLib.Units.Ship;
using ClassLib.Units.Bullet;

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

        //next level  
        private int x()
        {
            return Process.GetCurrentProcess().Id / 40;
        }
        private int y()
        {
            return Process.GetCurrentProcess().Id / 40;
        }
        //


        public Map(HubConnection hubConnection, int matchId, string playerName, int startingId, int randomY, Game mainMenu)
        {
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _isForcedToLeave = false;
            _mainMenu = mainMenu;
            _bullets = new Dictionary<int, PictureBox>();

            InitializeComponent(2);

            player = ships[0];
            player.Label.Text = playerName;
            player.Image.Location = new Point(100 + (startingId * 500), randomY);
            player.Label.Location = new Point(player.Image.Location.X, player.Image.Location.Y - 50);
            player.Image.Image = Properties.Resources.shipRight;

            this._matchId = matchId;
            this._playerName = playerName;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;


            _hubConnection = hubConnection;

            // for now its 1v1, so ships[1] is opponent and ships[0] is player

            _hubConnection.On<string, string, int, int>("LocationInfo", (shipName, facing, xAxis, yAxis) =>
            {
                opponent = ships[1];
                opponent.Image.Location = new Point(xAxis, yAxis);
                opponent.Label.Location = new Point(xAxis, yAxis - 50);
                opponent.Label.Text = shipName;
                switch (facing)
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
                opponent.Image.Image = facing.Equals("right") ? Properties.Resources.shipRight : Properties.Resources.shipLeft;
            });


            _hubConnection.On<int, int, int>("BulletLocationInfo", (bulletId, xAxis, yAxis) =>
            {
                if (!_bullets.ContainsKey(bulletId))
                {
                    PictureBox bullet = new PictureBox();
                    bullet.BackColor = System.Drawing.Color.Wheat; // set the colour white for the bullet
                    bullet.Size = new Size(5, 5); // set the size to the bullet to 5 pixel by 5 pixel
                    bullet.Tag = "bullet"; // set the tag to bullet
                    bullet.BringToFront(); // bring the bullet to front of other objects
                    this.Controls.Add(bullet); // add the bullet to the screen
                    _bullets[bulletId] = bullet;
                }
                PictureBox currentBullet = _bullets[bulletId];
                currentBullet.Location = new Point(xAxis, yAxis);
            });

            _hubConnection.On("LeaveMatch", () =>
            {
                _isForcedToLeave = true;
                this.Close();
            });
        }

        bool moveRight, moveLeft, moveUp, moveDown;
        int speed = 10;
        string facing = "up";


        private async void moveTimerEvent(object sender, EventArgs e)
        {
            if (moveLeft && player.Image.Left > 0)
            {
                player.Image.Left -= speed;
                player.Label.Left -= speed;
            }

            if (moveRight && player.Image.Right < 700)
            {
                player.Image.Left += speed;
                player.Label.Left += speed;
            }

            if (moveUp && player.Image.Top > 0)
            {
                player.Image.Top -= speed;
                player.Label.Top -= speed;
            }
            if (moveDown && player.Image.Top < 700)
            {
                player.Image.Top += speed;
                player.Label.Top += speed;
            }

            await _hubConnection.SendAsync("SendLocation", _matchId, _playerName, facing, player.Image.Location.X, player.Image.Location.Y);

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
                player.Image.Image = Properties.Resources.shipDown;
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
                facing = "up";
                player.Image.Image = Properties.Resources.shipUp;
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
                await _hubConnection.SendAsync("LeftMatch");
            }

            _mainMenu.Visible = true;
        }
    }
}