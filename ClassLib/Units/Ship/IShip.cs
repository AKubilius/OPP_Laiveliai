using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Ship
{
    public interface IShip
    {
        public Skin GetSkin();
    }

    public enum Skin
    {
        White,
        Red
    }
}
