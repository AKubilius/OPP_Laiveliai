using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    internal class Ship
    {
        public PictureBox ShipImage { get; set; }
        public Label ShipLabel { get; set; }

        public Ship(string shipName)
        {
            this.ShipImage = new PictureBox();
            this.ShipImage.BackColor = System.Drawing.Color.Transparent;
            this.ShipImage.Image = global::Client.Properties.Resources.right;
            this.ShipImage.Location = new System.Drawing.Point(428, 287);
            this.ShipImage.Name = shipName;
            this.ShipImage.Size = new System.Drawing.Size(56, 36);
            this.ShipImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ShipImage.TabIndex = 0;
            this.ShipImage.TabStop = false;

            this.ShipLabel = new Label();
            this.ShipLabel.AutoSize = true;
            this.ShipLabel.Location = new System.Drawing.Point(ShipImage.Location.X, ShipImage.Location.Y - 50);
            this.ShipLabel.Name = "label" + shipName;
            this.ShipLabel.Size = new System.Drawing.Size(35, 13);
            this.ShipLabel.TabIndex = 0;
            this.ShipLabel.Text = "label2";
        }

    }
}
