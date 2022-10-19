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

        public struct Authentication
        {
            public string PlayerName { get; set; }
            public string Response { get; set; }

            public Authentication(string playerName, string response)
            {
                PlayerName = playerName;
                Response = response;
            }
        }

        public struct MatchEvents
        {
            public string Response { get; set; }


            public MatchEvents(string response)
            {
                Response = response;
            }
        }

        public struct Matchmaking
        {
            public string Response { get; set; }
            public int StartingID { get; set; }
            public int StartingYPos { get; set; }


            public Matchmaking(string response, int startingID, int startingYPos)
            {
                Response = response;
                StartingID = startingID;
                StartingYPos = startingYPos;
            }
        }

        public struct Location
        {
            public string Response { get; set; }
            public string ShipName { get; set; }
            public string Facing { get; set; } = string.Empty;
            public int BulletID { get; set; } = int.MinValue;
            public int XAxis { get; set; }
            public int YAxis { get; set; }


            public Location(string response, string shipName, string facing, int xAxis, int yAxis)
            {
                Response = response;
                ShipName = shipName;
                Facing = facing;
                XAxis = xAxis;
                YAxis = yAxis;
            }

            public Location(string response, string shipName, int bulletID, int xAxis, int yAxis)
            {
                Response = response;
                ShipName = shipName;
                BulletID = bulletID;
                XAxis = xAxis;
                YAxis = yAxis;
            }
        }
    }
}
