using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Composite
{
    public class FrontArmour : Component
    {
        private string _name;
        public PictureBox _image;
        public Ship _parentShip;
        private string _facing;
        private Bitmap _defaultImage;
        public FrontArmour(string name, Ship ship, Bitmap bitmap)
        {
            _facing = "left";
            _parentShip = ship;
            _image = new PictureBox();
            _defaultImage = new Bitmap(bitmap, new Size(bitmap.Width / 10, bitmap.Height / 10));
            _image.Image = _defaultImage;
            _image.Location = new Point(_parentShip.Image.Location.X, _parentShip.Image.Location.Y);
            _image.Size = new Size(20, 20);
            _name = name;
        }
        void Component.Move()
        {
            Image img;
            switch (_parentShip.Facing)
            {
                case "left":
                    if (_facing != "left")
                    {
                        img = new Bitmap(_defaultImage);
                        _image.Image = img;
                        _facing = "left";
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X - 2, _parentShip.Image.Location.Y + 20);
                    break;
                case "right":
                    if (_facing != "right")
                    {
                        img = new Bitmap(_defaultImage);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        _image.Image = img;
                        _facing = "right";
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X + 40, _parentShip.Image.Location.Y +20);
                    break;
                case "up":
                    if(_facing != "up")
                    {
                        img = new Bitmap(_defaultImage);
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        _image.Image = img;
                        _facing = "up";
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X + 38, _parentShip.Image.Location.Y - 5);
                    break;
                case "down":
                    if (_facing != "down")
                    {
                        img = new Bitmap(_defaultImage);
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        _image.Image = img;
                        _facing = "down";
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X, _parentShip.Image.Location.Y + 30);
                    break;
            }
        }
        public string GetName()
        {
            return _name;
        }
    }
}
