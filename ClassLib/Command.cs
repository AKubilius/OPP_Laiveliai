using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Command
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Command(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public struct Register
        {
            public string PlayerName { get; set; }
            public string Response { get; set; }

            public Register(string playerName, string response)
            {
                PlayerName = playerName;
                Response = response;
            }
        }

        public struct Matchmaking
        {
            public string Response { get; set; }
            public int MatchID { get; set; }
            public int StartingID { get; set; }
            public int StartingYPos { get; set; }


            public Matchmaking(string response, int matchID, int startingID, int startingYPos)
            {
                Response = response;
                MatchID = matchID;
                StartingID = startingID;
                StartingYPos = startingYPos;
            }
        }
    }
}
