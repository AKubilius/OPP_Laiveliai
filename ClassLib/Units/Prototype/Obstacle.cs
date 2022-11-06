using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Obstacle
{
    public class Obstacle: IObstacle
    {
        public PictureBox Image;
        public Land land;
        public Obstacle()
        {
            this.Image = new PictureBox();
            this.Image.BackColor = System.Drawing.Color.SandyBrown;
            this.Image.Size = new System.Drawing.Size(56, 36);
            this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image.TabIndex = 1;
            this.Image.TabStop = false;
            this.Image.Location = new System.Drawing.Point(500, 560);
            this.Image.Tag = "Obstacle";

        }

        public Obstacle ShallowCopy()
        {
            return (Obstacle)this.MemberwiseClone();
        }

        public Obstacle DeepCopy()
        {

            Obstacle clone = (Obstacle)this.MemberwiseClone();
            
            clone.Image.BackColor = this.Image.BackColor;
            clone.Image.Size = this.Image.Size;
            clone.land = new Land(land.x, land.y) ;
            clone.Image.Location = new System.Drawing.Point(land.x, land.y);
            return clone;
        }

    }
}
