using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Obstacle
{
    internal interface IObstacle
    {
        Obstacle ShallowCopy();
        Obstacle DeepCopy();
       
    }
}
