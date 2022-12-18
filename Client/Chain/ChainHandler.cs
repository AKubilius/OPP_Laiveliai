using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Chain
{
    public abstract class ChainHandler
    {
        protected ChainHandler _successor;

        public void SetSuccessor(ChainHandler successor)
        {
            _successor = successor;
        }

        public abstract void ProcessRequest(int playerHealth, int bulletDamage, string playerName, Map map);
    }
}
