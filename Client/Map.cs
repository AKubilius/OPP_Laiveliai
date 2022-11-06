    using System.ComponentModel;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using ClassLib.Units.Ship;
using ClassLib.Units.Bullet;
using ClassLib;
using ClassLib.Builder;
using static ClassLib.Command;
using Newtonsoft.Json;
using ClassLib.Strategy;
using ClassLib.Units;
using ClassLib.Units.Weapons;

namespace Client
{
    public partial class Map : Form, ClassLib.Observer.IObserver
    {
        HubConnection _hubConnection;
        string _playerName;
        internal Ship player;
        Game _mainMenu;
        internal Dictionary<int, PictureBox> bullets;
        Bitmap _image;
        Keys _key;
        Skin _skin;

        Director _director;
        ShipBuilder _shipBuilder;

        CommandExecutor _commandExecutor;

        public Map(HubConnection hubConnection, string playerName, int startingId, int randomY, Game mainMenu)
        {
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _mainMenu = mainMenu;
            _director = new Director();
            _shipBuilder = new ShipBuilder();
            bullets = new Dictionary<int, PictureBox>();

            InitializeComponent();

            _skin = _mainMenu.GetSkin();
            NewShip(playerName, _skin);

            player = ships[playerName].Ship;
            player.Label.Text = playerName;
            player.Image.Location = new Point(100 + (startingId * 500), randomY);
            player.Health.Location = new Point(player.Image.Location.X + 6, player.Image.Location.Y + 50);
            player.Label.Location = new Point(player.Image.Location.X, player.Image.Location.Y - 50);

            this._playerName = playerName;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;



            _hubConnection = hubConnection;

            _commandExecutor = new MapCommandExecutorAdapter(this);

            _hubConnection.On<Command>("Message", (cmd) =>
            {
                _commandExecutor.Execute(cmd);
            });
        }

        public void RespondToEvent()
        {
            eventLabel.Text = "SOME EVENT STARTED, GET READY";
        }

        internal void ResetImage(ShipDecorator player)
        {
            Bitmap image = null;
            switch (player.GetSkin())
            {
                case Skin.White:
                    image = Properties.Resources.shipWhite;
                    break;
                case Skin.Red:
                    image = Properties.Resources.shipRed;
                    break;
                case Skin.Blue:
                    image = Properties.Resources.shipBlue;
                    break;
                case Skin.Yellow:
                    image = Properties.Resources.shipYellow;
                    break;
            }
            player.Ship.Image.Image = image;
        }

        internal Ship NewShip(string playerName, Skin skin)
        {
            Facade facade = new Facade(_director, _shipBuilder);

            Ship ship = facade.GetShip(playerName);

            switch (skin)
            {
                case Skin.White:
                    ships[playerName] = new ShipDecoratorWhite(ship);
                    break;
                case Skin.Red:
                    ships[playerName] = new ShipDecoratorRed(ship);
                    break;
                case Skin.Blue:
                    ships[playerName] = new ShipDecoratorBlue(ship);
                    break;
                case Skin.Yellow:
                    ships[playerName] = new ShipDecoratorYellow(ship);
                    break;
            }
            ResetImage(ships[playerName]);
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

            Location location = new Location("MovePlayer", _playerName, facing, player.Image.Location.X, player.Image.Location.Y, _skin);

            Command cmd = new Command("Location", JsonConvert.SerializeObject(location));
            await SendAsync(cmd);

        }
        private async void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != _key)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        ResetImage(ships[_playerName]);
                        player.Image.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case Keys.Right:
                        ResetImage(ships[_playerName]);
                        break;
                    case Keys.Down:
                        ResetImage(ships[_playerName]);
                        player.Image.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case Keys.Up:
                        ResetImage(ships[_playerName]);
                        player.Image.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
            }
            _key = e.KeyCode;
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                facing = "left";
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                facing = "right";
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

        private async void keyisup(object sender, KeyEventArgs e)
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
            if (e.KeyCode == Keys.Z)
            {
                ChangeWeapon();
            }
            if(e.KeyCode == Keys.P)
            {
                MatchEvent events = new MatchEvent("InitiateEvent", "");

                Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(events));
                await SendAsync(cmd);
            }

        }

        private void keypress(object sender, KeyPressEventArgs e)
        {

        }

        private async void keypress(object sender, KeyEventArgs e)
        {

        }

        private void ChangeWeapon()
        {
            if (player.GetStrategy() is AttackMachinegun)
            {
                WeaponName.Text = "Torpedo";
                player.SetStrategy(new AttackTorpedo());
            }
            else if (player.GetStrategy() is AttackTorpedo)
            {
                WeaponName.Text = "Laser";
                player.SetStrategy(new AttackLaser());
            }
            else if (player.GetStrategy() is AttackLaser)
            {
                WeaponName.Text = "Machinegun";
                player.SetStrategy(new AttackMachinegun());
            }
        }

        private void shoot(string direct)
        {
            // this is the function thats makes the new bullets in this game

            Weapon weaponBullet;

            if (player.GetStrategy() is AttackMachinegun)
            {
                weaponBullet = new Machinegun();
            }
            else
            { 
                if (player.GetStrategy() is AttackTorpedo)
                    weaponBullet = new Torpedo();
                else
                    weaponBullet = new Laser();
            }


            weaponBullet.direction = direct; // assignment the direction to the bullet
            weaponBullet.bulletLeft = player.Image.Left + (player.Image.Width / 2); // place the bullet to left half of the player
            weaponBullet.bulletTop = player.Image.Top + (player.Image.Height / 2); // place the bullet on top half of the player

            player.Shoot(weaponBullet, this, _playerName, _hubConnection, DateTime.UtcNow.GetHashCode());
        }

        private async void Map_Closing(object sender, CancelEventArgs e)
        {
            MatchEvent match = new MatchEvent("LeftMatch", _playerName);
            Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(match));
            await SendAsync(cmd);

            _mainMenu.Visible = true;
        }
    }
}