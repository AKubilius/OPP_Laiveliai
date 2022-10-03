using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Client
{
    internal class Ship
    {
        public PictureBox ShipImage { get; set; }
        public Label ShipLabel { get; set; }


        public int CoordinateGenerator()
        {
            Random rnd = new Random();
            int cord = rnd.Next(650);
            return cord;
        }

        public Ship(string shipName)
        {
            this.ShipImage = new PictureBox();
            this.ShipImage.BackColor = System.Drawing.Color.Transparent;
            this.ShipImage.Image = global::Client.Properties.Resources.right;
            this.ShipImage.Name = shipName;
            this.ShipImage.Size = new System.Drawing.Size(56, 36);
            this.ShipImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ShipImage.TabIndex = 0;
            this.ShipImage.TabStop = false;
            this.ShipLabel = new Label();
            this.ShipLabel.AutoSize = true;
            this.ShipLabel.Name = "label" + shipName;
            this.ShipLabel.Size = new System.Drawing.Size(35, 13);
            this.ShipLabel.TabIndex = 0;
            this.ShipLabel.Text = "label2";
            this.ShipLabel.BorderStyle = BorderStyle.FixedSingle;
            this.ShipLabel.ForeColor = System.Drawing.Color.White;
        }

    }
}
