using ClassLib;
using ClassLib.Composite;
using ClassLib.Units.Ship;
using Newtonsoft.Json;
using System.Numerics;
using System.Windows.Forms;
using static ClassLib.Command;

namespace Client
{
    internal class MapCommandExecutorAdapter : CommandExecutor
    {
        private readonly Map _map;

        public MapCommandExecutorAdapter(Map map)
        {
            _map = map;
        }

        public override Task Execute(Command command)
        {
            switch (command.Name)
            {
                case "Location":
                    UpdateLocation(command);
                    break;
                case "Armour":
                    AddArmour(command);
                    break;
                case "MatchEvent":
                    Events(command);
                    break;
            }
            return Task.CompletedTask;
        }

        private void Events(Command cmd)
        {
            MatchEvent events = JsonConvert.DeserializeObject<MatchEvent>(cmd.Content);

            switch (events.Response)
            {
                case "MapEvent":
                    MapEvent(events.PlayerName);
                    break;
                case "GlobalEvent":
                    GlobalEvent();
                    break;
                case "LeftMatch":
                    LeftMatch(events.PlayerName);
                    break;
            }
        }

        private void AddArmour(Command cmd)
        {
            Armour armour = JsonConvert.DeserializeObject<Armour>(cmd.Content);
            Ship opponent = _map.flyweightShipFactory.GetShip(armour.PlayerName).Ship;
            switch (armour.Type)
            {
                case "All":
                    FrontArmour fA = new FrontArmour("Front Ram", opponent, Properties.Resources.front_armour);
                    BackArmour bA = new BackArmour("Back Armour", opponent, Properties.Resources.back_armour);
                    MiddleArmour mA = new MiddleArmour("Middle Armour", opponent, Properties.Resources.middle_armour);
                    _map.Controls.Add(bA._image);
                    _map.Controls.Add(fA._image);
                    _map.Controls.Add(mA._image);
                    _map.Controls.SetChildIndex(bA._image, 0);
                    _map.Controls.SetChildIndex(fA._image, 0);
                    _map.Controls.SetChildIndex(mA._image, 0);
                    ((Armoury)opponent.WholeArmoury.GetChild("Back Armoury")).AddArmoury(bA);
                    ((Armoury)opponent.WholeArmoury.GetChild("Front Armoury")).AddArmoury(fA);
                    ((Armoury)opponent.WholeArmoury.GetChild("Middle Armoury")).AddArmoury(mA);
                    break;
                case "Front":
                    fA = new FrontArmour("Front Ram", opponent, Properties.Resources.front_armour);
                    _map.Controls.Add(fA._image);
                    _map.Controls.SetChildIndex(fA._image, 0);
                    ((Armoury)opponent.WholeArmoury.GetChild("Front Armoury")).AddArmoury(fA);
                    break;
                case "Back":
                    bA = new BackArmour("Back Armour", opponent, Properties.Resources.back_armour);
                    _map.Controls.Add(bA._image);
                    _map.Controls.SetChildIndex(bA._image, 0);
                    ((Armoury)opponent.WholeArmoury.GetChild("Back Armoury")).AddArmoury(bA);
                    break;
                case "Middle":
                    mA = new MiddleArmour("Middle Armour", opponent, Properties.Resources.middle_armour);
                    _map.Controls.Add(mA._image);
                    _map.Controls.SetChildIndex(mA._image, 0);
                    ((Armoury)opponent.WholeArmoury.GetChild("Middle Armoury")).AddArmoury(mA);
                    break;
            }
        }
        private void UpdateLocation(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "UpdateLocation":
                    UpdateLocation(location);
                    break;

                case "UpdateBulletLocation":
                    UpdateBulletLocation(location);
                    break;
            }
        }

        private void LeftMatch(string playerName)
        {
            _map.flyweightShipFactory.RemoveShip(playerName);
        }

        private void GlobalEvent()
        {
            _map.RespondToEvent();
        }

        private void MapEvent(string argument)
        {
            _map.MapEvent(argument);
        }


        private void UpdateBulletLocation(Location location)
        {
            if (!_map.bullets.ContainsKey(location.BulletID))
            {
                PictureBox bullet = new PictureBox();
                bullet.BackColor = System.Drawing.Color.Red; // set the colour red for the bullet
                bullet.Size = new Size(location.BulletWidth, location.BulletHeight); // set the size to the bullet to 5 pixel by 5 pixel
                bullet.Tag = "bullet"; // set the tag to bullet
                bullet.BringToFront(); // bring the bullet to front of other objects
                _map.Controls.Add(bullet); // add the bullet to the screen
                _map.bullets[location.BulletID] = bullet;
            }
            PictureBox currentBullet = _map.bullets[location.BulletID];
            currentBullet.Location = new Point(location.XAxis, location.YAxis);

            if (currentBullet.Bounds.IntersectsWith(_map.player.Image.Bounds))
            {
                if (_map.player.Health.Value > 1)
                    _map.player.Health.Value -= location.BulletDamage;
                _map.Controls.Remove(currentBullet);
            }
        }

        private void UpdateLocation(Location location)
        {
            Ship opponent = _map.flyweightShipFactory.GetShip(location.ShipName, (Skin)location.Skin).Ship;
            _map.ResetImage(_map.flyweightShipFactory.GetShip(location.ShipName));
            opponent.Image.Location = new Point(location.XAxis, location.YAxis);
            opponent.Label.Location = new Point(location.XAxis, location.YAxis - 50);
            opponent.Health.Location = new Point(location.XAxis + 6, location.YAxis + 50);
            opponent.Facing = location.Facing;
            opponent.WholeArmoury.Move();
            opponent.Label.Text = location.ShipName;
            switch (location.Facing)
            {
                case "right":
                    break;
                case "left":
                    opponent.Image.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case "up":
                    opponent.Image.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case "down":
                    opponent.Image.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                default:
                    break;
            }
        }
    }
}
