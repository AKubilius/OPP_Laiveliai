using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Builder
{
    public class ShipBuilder : IBuilder
    {
        private Ship _ship = new Ship();

        public ShipBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _ship = new Ship();
        }

        public void AddBody(string shipName)
        {
            _ship.Health = new ProgressBar();
            _ship.Health.Name = "Health";
            _ship.Health.Size = new Size(40, 6);
            _ship.Health.TabIndex = 0;
            _ship.Health.Value = Convert.ToInt32(100);
            _ship.Image = new PictureBox();
            _ship.Image.BackColor = System.Drawing.Color.Transparent;
            _ship.Image.Name = shipName;
            _ship.Image.Size = new System.Drawing.Size(56, 36);
            _ship.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            _ship.Image.TabIndex = 0;
            _ship.Image.TabStop = false;
            _ship.Label = new Label();
            _ship.Label.AutoSize = true;
            _ship.Label.Name = "label" + shipName;
            _ship.Label.Size = new System.Drawing.Size(35, 13);
            _ship.Label.TabIndex = 0;
            _ship.Label.Text = "label2";
            _ship.Label.BorderStyle = BorderStyle.FixedSingle;
            _ship.Label.ForeColor = System.Drawing.Color.White;
        }

        public void AddPower()
        {
            _ship.Power = 100;
        }

        public void AddSpeed()
        {
            _ship.Speed = 10;
        }

        public Ship GetShip()
        {
            Ship result = _ship;
            Reset();
            return result;
        }
    }
}
