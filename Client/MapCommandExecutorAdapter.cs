using ClassLib;
using ClassLib.Units.Ship;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                case "LeftMatch":
                    if (_map.ships.ContainsKey(command.Content))
                    {
                        var ship = _map.ships[command.Content].Ship;
                        _map.ships.Remove(command.Content);
                        ship.Dispose();
                    }
                    break;
            }
            return Task.CompletedTask;
        }

        public void UpdateLocation(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "UpdateLocation":
                    Ship opponent = null;
                    if (!_map.ships.ContainsKey(location.ShipName))
                    {
                        opponent = _map.NewShip(location.ShipName, (Skin)location.Skin);
                    }
                    else
                    {
                        opponent = _map.ships[location.ShipName].Ship;
                        _map.ResetImage(_map.ships[location.ShipName]);
                    }
                    opponent.Image.Location = new Point(location.XAxis, location.YAxis);
                    opponent.Label.Location = new Point(location.XAxis, location.YAxis - 50);
                    opponent.Health.Location = new Point(location.XAxis + 6, location.YAxis + 50);
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
                    break;

                case "UpdateBulletLocation":
                    if (!_map.bullets.ContainsKey(location.BulletID))
                    {
                        PictureBox bullet = new PictureBox();
                        bullet.BackColor = System.Drawing.Color.Red; // set the colour white for the bullet
                        bullet.Size = new Size(5, 5); // set the size to the bullet to 5 pixel by 5 pixel
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
                            _map.player.Health.Value -= 5;
                        _map.Controls.Remove(currentBullet);
                    }

                    break;
            }
        }
    }
}
