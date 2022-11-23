using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Factory
{
    internal class SandObstacle : Obstacle
    {
        PictureBox pb;
        public SandObstacle()
        {
            this.pb = new PictureBox();
            this.pb.BackColor = Color.SandyBrown;
            this.pb.BackColor = System.Drawing.Color.SandyBrown;
            this.pb.Size = new System.Drawing.Size(40, 40);
            this.pb.TabIndex = 1;
            this.pb.TabStop = false;
            this.pb.Location = new System.Drawing.Point(200, 300);
            this.pb.Tag = "Obstacle";
            SetImage(this.pb);
        }
    }
}
