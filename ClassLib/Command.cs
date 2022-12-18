using ClassLib.Units.Ship;
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

        public struct MatchEvent
        {
            public string Response { get; set; }
            public string PlayerName { get; set; }

            public MatchEvent(string response, string playerName)
            {
                Response = response;
                PlayerName = playerName;
            }

        }

        public struct Armour
        {
            public string Type { get; set; }
            public string PlayerName { get; set; }

            public Armour(string type, string playerName)
            {
                Type = type;
                PlayerName = playerName;
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
            public int PlayerHealth { get; set; } = 0;
            public string Facing { get; set; } = string.Empty;
            public string BulletType { get; set; } = string.Empty;
            public int BulletID { get; set; } = int.MinValue;
            public int BulletDamage { get; set; } = 0;
            public int BulletWidth { get; set; } = 0;
            public int BulletHeight { get; set; } = 0;
            public int XAxis { get; set; }
            public int YAxis { get; set; }
            public Skin? Skin { get; set; }


            public Location(string response, string shipName, string facing, int xAxis, int yAxis, Skin skin)
            {
                Response = response;
                ShipName = shipName;
                Facing = facing;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = skin;
            }

            public Location(string response, int playerHealth, string shipName, string facing, int xAxis, int yAxis, Skin skin)
            {
                Response = response;
                PlayerHealth = playerHealth;
                ShipName = shipName;
                Facing = facing;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = skin;
            }

            public Location(string response, int playerHealth, string shipName, int xAxis, int yAxis)
            {
                Response = response;
                PlayerHealth = playerHealth;
                ShipName = shipName;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = null;
            }

            public Location(string response, string shipName, string bulletType, int bulletID, int bulletSizeWidth, int bulletSizeHeight, int xAxis, int yAxis)
            {
                Response = response;
                ShipName = shipName;
                BulletID = bulletID;
                BulletWidth = bulletSizeWidth;
                BulletHeight = bulletSizeHeight;
                BulletType = bulletType;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = null;
            }

            public Location(string response, int playerHealth, string shipName, string bulletType, int bulletID, int bulletSizeWidth, int bulletSizeHeight, int xAxis, int yAxis)
            {
                Response = response;
                ShipName = shipName;
                PlayerHealth = playerHealth;
                BulletID = bulletID;
                BulletWidth = bulletSizeWidth;
                BulletHeight = bulletSizeHeight;
                BulletType = bulletType;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = null;
            }

            public Location(string response, string shipName, int bulletDamage, string bulletType, int bulletID, int bulletSizeWidth, int bulletSizeHeight, int xAxis, int yAxis)
            {
                Response = response;
                ShipName = shipName;
                BulletID = bulletID;
                BulletWidth = bulletSizeWidth;
                BulletHeight = bulletSizeHeight;
                BulletType = bulletType;
                BulletDamage = bulletDamage;
                XAxis = xAxis;
                YAxis = yAxis;
                Skin = null;
            }
        }
    }
}
