using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.State;

namespace ClassLib.Memento
{
    public class MapMemento
    {
        State.State state;

        public State.State State
        { 
            get { return state; }
            set { state = value; }
        }
    }
}
