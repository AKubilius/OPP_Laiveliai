using ClassLib;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Server.Models;
using static ClassLib.Command;

namespace Server.Hubs
{
    public class Game : Hub
    {
        private static List<Player> _registeredPlayers = new List<Player>();
        private static List<Match> _matches = new List<Match>();
        private static List<Player> _playersInMatchmaking = new List<Player>();
        static private Random random = new Random();

        private object _lockerRegisteredPlayers = new object();
        private object _lockerMatchmaking = new object();
        private object _lockerMatches = new object();


        public async void Message(Command cmd)
        {
            switch (cmd.Name)
            {
                case "Register":
                    await RegisterPlayer(cmd);
                    break;

                case "Matchmaking":
                    await Matchmaking(cmd);
                    break;
            }
        }

        private async Task SendAsync(Command cmd, string callerID)
        {
            await Clients.Client(callerID).SendAsync("Message", cmd);
        }


        public async Task RegisterPlayer(Command cmd)
        {
            Register register = JsonConvert.DeserializeObject<Register>(cmd.Content);

            bool isSuccesful = false;
            lock (_lockerRegisteredPlayers)
            {
                var player = _registeredPlayers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                if (player == null)
                {
                    Player temp = _registeredPlayers.FirstOrDefault(x => x.Name.Equals(register.PlayerName));
                    if (temp == null)
                    {
                        player = new Player { ConnectionId = Context.ConnectionId, Name = register.PlayerName };
                        _registeredPlayers.Add(player);
                        isSuccesful = true;
                    }
                }
            }

            if (isSuccesful)
            {
                register.Response = "RegisterComplete";
                Command answer = new Command("Register", JsonConvert.SerializeObject(register));
                await SendAsync(answer, Context.ConnectionId);
            }
            else
            {
                register.Response = "NameOccupied";
                Command answer = new Command("Register", JsonConvert.SerializeObject(register));
                await SendAsync(answer, Context.ConnectionId);
            }

        }

        //public void LogoutPlayer()
        //{
        //    lock (_lockerRegisteredPlayers)
        //    {
        //        var player = _registeredPlayers.First(x => x.ConnectionId == Context.ConnectionId);

        //        if (player != null)
        //        {
        //            lock (_lockerMatchmaking)
        //            {
        //                _playersInMatchmaking.Remove(player);
        //            }
        //            _registeredPlayers.Remove(player);
        //        }
        //    }
        //}

        //public async void LeftMatch()
        //{
        //    Match match = null;
        //    lock (_matches)
        //    {
        //        match = _matches.First(x => x.Players.Any(y => y.ConnectionId == Context.ConnectionId));

        //        if (match != null)
        //        {
        //            _matches.Remove(match);
        //        }
        //    }

        //    if (match != null)
        //    {
        //        foreach (Player player in match.Players)
        //        {
        //            if (player.ConnectionId != Context.ConnectionId)
        //                await Clients.Client(player.ConnectionId).SendAsync("LeaveMatch");
        //        }
        //    }
        //}

        //public async void SendLocation(int matchId, string playerName, string facing, int xAxis, int yAxis)
        //{
        //    Match match = null;
        //    lock (_lockerMatches)
        //    {

        //        foreach (Match m in _matches)
        //        {
        //            foreach (Player p in m.Players)
        //            {
        //                if (p.Name.Equals(playerName))
        //                {
        //                    match = m;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    Player opponent = null;
        //    if (match != null)
        //        opponent = match.Players.First(x => x.Name != playerName);
        //    if (opponent != null)
        //        await Clients.Client(opponent.ConnectionId).SendAsync("LocationInfo", playerName, facing, xAxis, yAxis);
        //}
        //public async void SendBulletLocation(int matchId, string playerName, int bulletId, int xAxis, int yAxis)
        //{
        //    Match match = null;
        //    lock (_lockerMatches)
        //    {

        //        foreach (Match m in _matches)
        //        {
        //            foreach (Player p in m.Players)
        //            {
        //                if (p.Name.Equals(playerName))
        //                {
        //                    match = m;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    Player opponent = null;
        //    if (match != null)
        //        opponent = match.Players.First(x => x.Name != playerName);
        //    if (opponent != null)
        //        await Clients.Client(opponent.ConnectionId).SendAsync("BulletLocationInfo", bulletId, xAxis, yAxis);
        //}

        public async Task Matchmaking(Command cmd)
        {
            Matchmaking matchmaking = JsonConvert.DeserializeObject<Matchmaking>(cmd.Content);

            switch (matchmaking.Response)
            {
                case "JoinMatchmaking":
                    await JoinMatchmaking(Context.ConnectionId);
                    break;

                // not implemented yet
                case "LeaveMatchmaking":
                    break;
            }
        }

        public async Task JoinMatchmaking(string callerID)
        {
            Matchmaking matchmaking = new Matchmaking();
            Command answer = null;

            var player = _registeredPlayers.FirstOrDefault(x => x.ConnectionId == callerID);
            if (player == null) return;

            Player opponent = null;
            lock (_lockerMatchmaking)
            {
                _playersInMatchmaking.Add(player);
                opponent = _playersInMatchmaking.FirstOrDefault(x => x.ConnectionId != player.ConnectionId);
            }

            if (opponent == null)
            {
                matchmaking.Response = "InMatchmakingQueue";
                answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
                await SendAsync(answer, player.ConnectionId);

                return;
            }

            player.Opponent = opponent;
            opponent.Opponent = player;

            lock (_lockerMatchmaking)
            {
                _playersInMatchmaking.Remove(opponent);
                _playersInMatchmaking.Remove(player);
            }

            var match = new Match { Players = new List<Player> { player, opponent }, MatchId = DateTime.UtcNow.GetHashCode() };

            lock (_lockerMatches)
            {
                _matches.Add(match);
            }


            matchmaking.Response = "MatchCreated";
            matchmaking.MatchID = match.MatchId;

            matchmaking.StartingID = 0;
            matchmaking.StartingYPos = random.Next(0, 60);
            answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
            await SendAsync(answer, player.ConnectionId);

            matchmaking.StartingID = 1;
            matchmaking.StartingYPos = random.Next(0, 600);
            answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
            await SendAsync(answer, opponent.ConnectionId);
        }
    }
}
