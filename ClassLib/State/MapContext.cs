using ClassLib.Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.State
{
    public class MapContext
    {
        State state;

        public MapContext(State state)
        {
            this.state = state;
        }

        public State State
        {
            get { return state; }
            set { state = value; }
        }

        public Command GetCommand()
        {
            return state.CreateCommand();
        }

        //*********************Memento*********************
        public MapMemento SaveMemento()
        {
            return new MapMemento() { State = state };
        }
        public void RestoreMemento(MapMemento mapMemento)
        {
            state = mapMemento.State;
        }
    }
}
