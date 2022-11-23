using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Factory
{
    internal class TreeObstacle : Obstacle
    {
        PictureBox pb;
        public TreeObstacle()
        {
            this.pb = new PictureBox();
            this.pb.BackColor = Color.Green;
            SetName("Tree");
            SetSpeed(2);
            this.pb.Size = new System.Drawing.Size(60, 10);
            this.pb.TabIndex = 1;
            this.pb.TabStop = false;
            this.pb.Location = new System.Drawing.Point(40, 700);
            this.pb.Tag = "Obstacle";
            SetImage(this.pb);
        }
    }
}
