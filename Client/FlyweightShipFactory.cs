using ClassLib;
using ClassLib.Builder;
using ClassLib.Units.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FlyweightShipFactory
    {
        private Dictionary<string, ShipDecorator> ships = new Dictionary<string, ShipDecorator>();
        Director _director;
        ShipBuilder _shipBuilder;
        Map _map;

        public FlyweightShipFactory(Map map)
        {
            _map = map;
            _director = new Director();
            _shipBuilder = new ShipBuilder();
        }

        public ShipDecorator GetShip(string name, Skin? skin = null)
        {
            if (ships.ContainsKey(name))
            {
                return ships[name];
            } 
            else
            {
                Facade facade = new Facade(_director, _shipBuilder);

                Ship ship = facade.GetShip(name);

                switch (skin)
                {
                    case Skin.White:
                        ships[name] = new ShipDecoratorWhite(ship);
                        break;
                    case Skin.Red:
                        ships[name] = new ShipDecoratorRed(ship);
                        break;
                    case Skin.Blue:
                        ships[name] = new ShipDecoratorBlue(ship);
                        break;
                    case Skin.Yellow:
                        ships[name] = new ShipDecoratorYellow(ship);
                        break;
                }
                ((System.ComponentModel.ISupportInitialize)(ship.Image)).BeginInit();
                _map.Controls.Add(ship.Image);
                _map.Controls.Add(ship.Label);
                _map.Controls.Add(ship.Health);
                ((System.ComponentModel.ISupportInitialize)(ship.Image)).EndInit();
                return ships[name];
            }
        }

        public void RemoveShip(string name)
        {
            if (ships.ContainsKey(name))
            {
                var ship = ships[name];
                ships.Remove(name);
                ship.Ship.Dispose();
            }
        }
    }
}
