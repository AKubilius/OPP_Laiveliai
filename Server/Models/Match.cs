using ClassLib;

namespace Server.Models
{
    public class Match
    {
        private static Match instance = null;
        private static object threadLock = new object();
        private Match()
        {

        }

        public static Match Instance()
        {
            lock (threadLock)
            {
                if (instance == null)
                {
                    instance = new Match();
                }
            }
            return instance;
        }
        public List<Player> Players { get; set; }
    }
}
