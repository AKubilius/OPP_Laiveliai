using ClassLib;
using ClassLib.Units.Ship;
using Client.Chain;
using Client.Properties;
using Newtonsoft.Json;
using static ClassLib.Command;

namespace Client
{
    public class MapCommandExecutorAdapter : CommandExecutor
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
                case "UpdateParticleLocation":
                    UpdateParticleLocation(location);
                    break;
            }
        }

        private void UpdateParticleLocation(Location location)
        {
             if(location.PlayerHealth > 5)
             {
                PictureBox boom = CreateBoom(location.XAxis, location.YAxis);

                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        _map.Invoke(new Action(() => AddBoom(boom, _map)));
                        Thread.Sleep(1250);
                        _map.Invoke(new Action(() => RemoveBoom(boom, _map)));
                    });
                }
                catch (Exception e)
                {

                }
             }
        }

        public PictureBox CreateBoom(int xAxis, int yAxis)
        {
            PictureBox boom = new PictureBox();
            boom.Size = new Size(100, 100);
            boom.Location = new Point(xAxis, yAxis);
            boom.SizeMode = PictureBoxSizeMode.StretchImage;
            boom.BackColor = Color.Transparent;
            Bitmap gif = Resources.boomHit;
            boom.Image = (Image)gif;
            return boom;
        }

        public void AddBoom(PictureBox boom, Map map)
        {
            map.Controls.Add(boom);
            boom.BringToFront();
        }

        public void RemoveBoom(PictureBox boomImage, Map map)
        {
            map.Controls.Remove(boomImage);
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

            int playerHealth = _map.player.Health.Value;
            int bulletDamage = location.BulletDamage;
            string playerName = _map.player.Label.ToString();

            if (currentBullet.Bounds.IntersectsWith(_map.player.Image.Bounds))
            {
                _map.Controls.Remove(currentBullet);

                ChainHandler damageChecker = new DamageChecker();
                ChainHandler playerKicker = new PlayerKicker();

                damageChecker.SetSuccessor(playerKicker);
                damageChecker.ProcessRequest(playerHealth, bulletDamage, playerName, _map);
            }
        }

        private void UpdateLocation(Location location)
        {
            Ship opponent = _map.flyweightShipFactory.GetShip(location.ShipName, (Skin)location.Skin).Ship;
            _map.ResetImage(_map.flyweightShipFactory.GetShip(location.ShipName));
            opponent.Image.Location = new Point(location.XAxis, location.YAxis);
            opponent.Label.Location = new Point(location.XAxis, location.YAxis - 50);
            opponent.Health.Hide();
            opponent.Label.Text = location.ShipName;
            opponent.Label.ForeColor = Color.Red;
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
