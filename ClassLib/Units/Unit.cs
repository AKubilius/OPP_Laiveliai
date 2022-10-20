using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLib.Units
{
    public class Unit : IDisposable
    {
        public PictureBox Image { get; set; }
        public Label Label { get; set; }

        public virtual void Dispose()
        {
            Image.Dispose();
            Label.Dispose();
        }
    }
}
