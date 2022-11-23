using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Factory
{
    public class ObstacleFactory
    {
        public Obstacle makeObstacle(string obstacleType)
        {
            Obstacle obstacle = null;

            if (obstacleType.Equals("W"))
                return new WallObstacle();
            if (obstacleType.Equals("S"))
                return new SandObstacle();
            if (obstacleType.Equals("T"))
                return new TreeObstacle();
            else
                return null;
                
        }

    }
}
