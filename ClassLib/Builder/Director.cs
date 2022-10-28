using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Builder
{
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

        public void BuildSimpleShip(string shipName)
        {
            _builder.AddSpeed();
            _builder.AddPower();
            _builder.AddBody(shipName);
            _builder.AddStrategy();
        }

        public void BuildNotMovingShip(string shipName)
        {
            _builder.AddPower();
            _builder.AddBody(shipName);
            _builder.AddStrategy();
        }
    }
}
