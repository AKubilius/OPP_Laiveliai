﻿using System;
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

        public struct MatchEvents
        {
            public string Response { get; set; }
            public int MatchID { get; set; }


            public MatchEvents(string response, int matchID)
            {
                Response = response;
                MatchID = matchID;
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
            
            public int MatchID { get; set; }
            public string ShipName { get; set; }
            public string Facing { get; set; }

            public int XAxis { get; set; }

            public int YAxis { get; set; }


            public Location(string response, int matchID, string shipName, string facing, int xAxis, int yAxis)
            {
                Response = response;
                MatchID = matchID;
                ShipName = shipName;
                Facing = facing;
                XAxis = xAxis;
                YAxis = yAxis;
            }
        }


        public struct BulletLocation
        {
            public string Response { get; set; }

            public int MatchID { get; set; }
            public string ShipName { get; set; }


            public int BulletID { get; set; }

            public int XAxis { get; set; }

            public int YAxis { get; set; }


            public BulletLocation(string response, int matchID, string shipName, int bulletID, int xAxis, int yAxis)
            {
                Response = response;
                MatchID = matchID;
                ShipName = shipName;
                BulletID = bulletID;
                XAxis = xAxis;
                YAxis = yAxis;
            }
        }
    }
}
