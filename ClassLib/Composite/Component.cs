using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Composite
{
    public interface Component
    {
        public void Move();
        public string GetName();
    }
}
