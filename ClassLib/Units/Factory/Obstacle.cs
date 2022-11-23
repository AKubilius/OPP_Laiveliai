using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Factory
{
    public abstract class Obstacle
    {
        private string Name;
        private int Speed;
        private PictureBox pictureBox;
        public string GetName() { return Name; }
       
        public void SetName(string name) { Name = name; }

        public PictureBox GetImage() { return pictureBox; }
        public void SetImage(PictureBox pb) { pictureBox = pb; }

        public int GetSpeed() { return Speed; }
        public void SetSpeed(int speed) {Speed = speed; }
        public void SayHello()
        {
            Console.WriteLine("stuck");
        }
    }
}
