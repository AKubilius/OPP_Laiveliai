using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Composite
{
    public class MiddleArmour : Component
    {
        private string _name;
        public PictureBox _image;
        public Ship _parentShip;
        private string _facing;
        private Bitmap _defaultImage;

        public MiddleArmour(string name, Ship ship, Bitmap bitmap)
        {
            _facing = "left";
            _parentShip = ship;
            _image = new PictureBox();
            _defaultImage = new Bitmap(bitmap, new Size(bitmap.Width / 9, bitmap.Height / 9));
            _image.Image = _defaultImage;
            _image.Location = new Point(_parentShip.Image.Location.X, _parentShip.Image.Location.Y);
            _image.Size = new Size(23, 20);
            _name = name;
        }

        public void Move()
        {
            Image img;
            switch (_parentShip.Facing)
            {
                case "left":
                    if (_facing != "left")
                    {
                        img = new Bitmap(_defaultImage);
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        _image.Image = img;
                        _facing = "left";
                        _image.Size = new Size(23, 20);
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X + 17, _parentShip.Image.Location.Y + 20);
                    break;
                case "right":
                    if (_facing != "right")
                    {
                        img = new Bitmap(_defaultImage, new Size(_defaultImage.Width +4, _defaultImage.Height));
                        _image.Image = img;
                        _facing = "right";
                        _image.Size = new Size(27, 20);
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X +13, _parentShip.Image.Location.Y + 20);
                    break;
                case "up":
                    if (_facing != "up")
                    {
                        img = new Bitmap(_defaultImage, new Size(_defaultImage.Width, _defaultImage.Height + 2));
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        _image.Image = img;
                        _image.Size = new Size(20, 15);
                        _facing = "up";
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X + 36, _parentShip.Image.Location.Y + 15);
                    break;
                case "down":
                    if (_facing != "down")
                    {
                        img = new Bitmap(_defaultImage, new Size(_defaultImage.Width, _defaultImage.Height +  2));
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        _image.Image = img;
                        _facing = "down";
                        _image.Size = new Size(20, 22);
                    }
                    _image.Location = new Point(_parentShip.Image.Location.X, _parentShip.Image.Location.Y + 9);
                    break;
            }
        }
        public string GetName()
        {
            return _name;
        }
    }
}
