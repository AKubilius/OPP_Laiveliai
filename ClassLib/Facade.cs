using ClassLib.Builder;
using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Facade
    {
        protected Director _director;
        protected ShipBuilder _shipBuilder;
        public Facade(Director director, ShipBuilder shipBuilder)
        {
            _director = director;
            _shipBuilder = shipBuilder;
            _director.Builder = _shipBuilder;
        }

        public Ship GetShip(string playerName)
        {
            _director.BuildSimpleShip(playerName);
            return _shipBuilder.GetShip();
        }

        public Ship GetNotMovingShip(string playerName)
        {
            _director.BuildNotMovingShip(playerName);
            return _shipBuilder.GetShip();
        }
    }
}
