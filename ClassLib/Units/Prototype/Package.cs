using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLib.Units.Prototype
{
    public class Package
    {
        private PictureBox Image;
        public Cover Cover; 
        public int Time;
            public Package(Cover cover, int time)
            {
            this.Image = new PictureBox();
            Cover = cover;
            Time = time;
                this.Image.BackColor = System.Drawing.Color.Black;
                this.Image.Size = new System.Drawing.Size(40, 20);
                this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.Image.TabIndex = 1;
                this.Image.TabStop = false;
                this.Image.Location = new System.Drawing.Point(300,200);
                this.Image.Tag = "Package";
            SetImage(this.Image);
            }

        public PictureBox getImage()
        {
            return Image;
        }
        public void SetImage(PictureBox pb) { Image = pb; }
        public Package ShallowCopy()
            {
                return (Package)this.MemberwiseClone();
            }

            public Package DeepCopy()
            {

                Package clone = (Package)this.MemberwiseClone();
            clone.Cover = new Cover
            {
                Url = this.Cover.Url,
                Id = this.Cover.Id
            };
                return (Package)clone;
            }
        public void Print(string Type)
        {
            Debug.WriteLine(Type);
            Debug.WriteLine("Atminties Adresas :" + this.GetHashCode());
            Debug.WriteLine("Time :" + this.Time);
            Debug.WriteLine("Cover url :" + this.Cover.Url + "Cover ID:" + this.Cover.Id); ;
        }
    }
}
