using ClassLib.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Builder
{
    public interface IBuilder
    {
        void AddBody(string shipName);
        void AddSpeed();
        void AddPower();
        void AddStrategy();
    }
}
