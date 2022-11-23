using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Factory
{
    public class WallObstacle : Obstacle
    {
         PictureBox pb;
         public WallObstacle()
        {
            SetName("Red");
            SetSpeed(1);
            this.pb = new PictureBox();
            this.pb.BackColor = Color.Red;
            this.pb.Size = new System.Drawing.Size(140, 50);
            this.pb.TabIndex = 1;
            this.pb.TabStop = false;
            this.pb.Location = new System.Drawing.Point(500, 560);
            this.pb.Tag = "Obstacle";
            SetImage(this.pb);
        }
    }
}
