using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Chain
{
    public class DamageChecker : ChainHandler
    {
        public override void ProcessRequest(int playerHealth, int bulletDamage, string playerName, Map map)
        {
            if (map.player.Health.Value <= bulletDamage)
            {
                _successor.ProcessRequest(playerHealth, bulletDamage, playerName, map);
            }
            else
            {
                map.player.Health.Value -= bulletDamage;
            }
        }
    }
}
