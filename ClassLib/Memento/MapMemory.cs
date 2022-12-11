using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Memento
{
    public class MapMemory
    {
        MapMemento mapMemento;

        public MapMemento MapMemento
        {
            get { return mapMemento; }
            set { mapMemento = value; }
        }
    }
}
