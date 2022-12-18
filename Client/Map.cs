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
using System.Runtime.InteropServices;
using ClassLib.State;
using ClassLib.Memento;

namespace Client
{
    public partial class Map : Form, ClassLib.Observer.IObserver
    {
        public HubConnection _hubConnection;
        public string _playerName;
        public Ship player;
        public Game _mainMenu;
        internal Dictionary<int, PictureBox> bullets;
        Bitmap _image;
        Keys _key;
        Skin _skin;

        //Director _director;
        //ShipBuilder _shipBuilder;

        internal FlyweightShipFactory flyweightShipFactory;

        CommandExecutor _commandExecutor;

        public Map(HubConnection hubConnection, string playerName, int startingId, int randomY, Game mainMenu)
        {
            if(playerName == "admin")
            {
               Thread thread = new Thread(() => ConsoleCreation());
                thread.Start();
            }
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _mainMenu = mainMenu;

            bullets = new Dictionary<int, PictureBox>();
            flyweightShipFactory = new FlyweightShipFactory(this);

            InitializeComponent();

            _skin = _mainMenu.GetSkin();
            //GetShip(playerName, _skin);

            player = flyweightShipFactory.GetShip(playerName, _skin).Ship;
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

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        async void ConsoleCreation()
        {
            AllocConsole();
            Console.WriteLine("Input the command (command arg1(optional), ... argX(optional)");
            MapContext mapContext = new MapContext(new State());
            MapMemory mapMemory = new MapMemory() { MapMemento = new MapMemento() { State = mapContext.State } };
            while (true)
            {
                var command = Console.ReadLine();
                string[] values = command.Split(" ");
                switch (values[0])
                {
                    case "ChangeMap":
                        if(values.Length <= 1)
                        {
                            Console.WriteLine("Invalid argument");
                            break;
                        }
                        switch (values[1])
                        {
                            case "map":
                                mapMemory.MapMemento = mapContext.SaveMemento();
                                mapContext.State = new State();
                                await SendAsync(mapContext.GetCommand());
                                break;
                            case "map1":
                                mapMemory.MapMemento = mapContext.SaveMemento();
                                mapContext.State = new LavaState();
                                await SendAsync(mapContext.GetCommand());
                                break;
                            case "map2":
                                mapMemory.MapMemento = mapContext.SaveMemento();
                                mapContext.State = new WavesState();
                                await SendAsync(mapContext.GetCommand());
                                break;
                            case "map3":
                                mapMemory.MapMemento = mapContext.SaveMemento();
                                mapContext.State = new CloudState();
                                await SendAsync(mapContext.GetCommand());
                                break;
                            case "previous":
                                State current = mapContext.State;
                                mapContext.RestoreMemento(mapMemory.MapMemento);
                                mapMemory.MapMemento = new MapMemento() { State = current };
                                await SendAsync(mapContext.GetCommand());
                                break;
                            default:
                                Console.WriteLine("Invalid argument");
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }

        public void RespondToEvent()
        {
            eventLabel.Text = "SOME EVENT STARTED, GET READY";
        }

        public void MapEvent(string map)
        {
            switch (map)
            {
                case "map1":
                    this.BackgroundImage = Properties.Resources.lava;
                    break;
                case "map2":
                    this.BackgroundImage = Properties.Resources.waves;
                    break;
                case "map3":
                    this.BackgroundImage = Properties.Resources.clouds;
                    break;
                case "map":
                    this.BackgroundImage = null;
                    break;

            }
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

        internal Ship GetShip(string playerName, Skin skin)
        {
            Ship ship = flyweightShipFactory.GetShip(playerName, skin).Ship;
            ResetImage(flyweightShipFactory.GetShip(playerName));
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


        public bool moveRight, moveLeft, moveUp, moveDown;
        int speed = 8;
        string facing = "up";

        public async void moveTimerEvent(object sender, EventArgs e)
        {
            if (moveLeft && player.Image.Left > 0)
            {
                player.Image.Left -= Math.Min(player.Image.Left, speed);
                player.Label.Left -= Math.Min(player.Image.Left, speed);
                player.Health.Left -= Math.Min(player.Image.Left, speed);
            }

            if (moveRight && player.Image.Left < ClientSize.Width)
            {
                player.Image.Left += Math.Min(ClientSize.Width - player.Image.Left, speed);
                player.Label.Left += Math.Min(ClientSize.Width - player.Image.Left, speed);
                player.Health.Left += Math.Min(ClientSize.Width - player.Image.Left, speed);
            }

            if (moveUp && player.Image.Top > 0)
            {
                player.Image.Top -= Math.Min(player.Image.Top, speed);
                player.Label.Top -= Math.Min(player.Image.Top, speed);
                player.Health.Top -= Math.Min(player.Image.Top, speed);
            }
            if (moveDown && player.Image.Top < ClientSize.Height)
            {
                player.Image.Top += Math.Min(ClientSize.Height - player.Image.Top, speed);
                player.Label.Top += Math.Min(ClientSize.Height - player.Image.Top, speed);
                player.Health.Top += Math.Min(ClientSize.Height - player.Image.Top, speed);
            }

            Location location = new Location("MovePlayer", player.Health.Value, _playerName, facing, player.Image.Location.X, player.Image.Location.Y, _skin);

            Command cmd = new Command("Location", JsonConvert.SerializeObject(location));
            await SendAsync(cmd);

        }
        public async void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != _key)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        ResetImage(flyweightShipFactory.GetShip(_playerName));
                        player.Image.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case Keys.Right:
                        ResetImage(flyweightShipFactory.GetShip(_playerName));
                        break;
                    case Keys.Down:
                        ResetImage(flyweightShipFactory.GetShip(_playerName));
                        player.Image.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case Keys.Up:
                        ResetImage(flyweightShipFactory.GetShip(_playerName));
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

            Weapon weaponBullet = new Weapon();

            weaponBullet.direction = direct; // assignment the direction to the bullet
            weaponBullet.bulletLeft = player.Image.Left + (player.Image.Width / 2); // place the bullet to left half of the player
            weaponBullet.bulletTop = player.Image.Top + (player.Image.Height / 2); // place the bullet on top half of the player

            player.Shoot(weaponBullet, this, _playerName, _hubConnection, DateTime.UtcNow.GetHashCode());
        }

        public async void Map_Closing(object sender, CancelEventArgs e)
        {
            MatchEvent match = new MatchEvent("LeftMatch", _playerName);
            Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(match));
            await SendAsync(cmd);

            _mainMenu.Visible = true;
        }

        //public async void Map_Closing(object sender, CancelEventArgs e)
        //{
        //    MatchEvent match = new MatchEvent("LeftMatch", _playerName);
        //    Command cmd = new Command("MatchEvent", JsonConvert.SerializeObject(match));
        //    await SendAsync(cmd);

        //    _mainMenu.Visible = true;
        //}
    }
}