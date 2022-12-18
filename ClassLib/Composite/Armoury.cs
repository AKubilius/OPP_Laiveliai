using ClassLib.Iterator;
using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Composite
{
    public class Armoury : Component
    {
        private CustomCollection<Component> _childArmouries;
        private string _name;
        public Armoury(string name)
        {
            _name = name;
            _childArmouries = new CustomCollection<Component>();
        }

        public void Move()
        {
            foreach (Component component in _childArmouries)
            {
                component.Move();
            }
        }

        public Component GetChild(string name)
        {
            foreach (Component component in _childArmouries)
            {
                if(component.GetName() == name)
                {
                    return component;
                }
            }

            return null;
        }

        public void AddArmoury(Component component)
        {
            _childArmouries.AddItem(component);
        }

        public string GetName()
        {
            return _name;
        }
    }
}
