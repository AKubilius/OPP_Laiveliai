using ClassLib;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
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
                case "Authentication":
                    await Authentication(cmd);
                    break;
                case "MatchEvents":
                    await MatchEvents(cmd);
                    break;
                case "Matchmaking":
                    await Matchmaking(cmd);
                    break;
                case "Location":
                    await Location(cmd);
                    break;
            }
        }

        private async Task SendAsync(Command cmd, string callerID)
        {
            await Clients.Client(callerID).SendAsync("Message", cmd);
        }


        public async Task Authentication(Command cmd)
        {
            Authentication auth = JsonConvert.DeserializeObject<Authentication>(cmd.Content);
            switch (auth.Response)
            {
                case "Register":
                    bool isSuccesful = false;
                    lock (_lockerRegisteredPlayers)
                    {
                        var player = _registeredPlayers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                        if (player == null)
                        {
                            Player temp = _registeredPlayers.FirstOrDefault(x => x.Name.Equals(auth.PlayerName));
                            if (temp == null)
                            {
                                player = new Player { ConnectionId = Context.ConnectionId, Name = auth.PlayerName };
                                _registeredPlayers.Add(player);
                                isSuccesful = true;
                            }
                        }
                    }

                    if (isSuccesful)
                    {
                        auth.Response = "RegisterComplete";
                        Command answer = new Command("Register", JsonConvert.SerializeObject(auth));
                        await SendAsync(answer, Context.ConnectionId);
                    }
                    else
                    {
                        auth.Response = "NameOccupied";
                        Command answer = new Command("Register", JsonConvert.SerializeObject(auth));
                        await SendAsync(answer, Context.ConnectionId);
                    }
                    break;
                case "Logout":
                    lock (_lockerRegisteredPlayers)
                    {
                        var player = _registeredPlayers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                        if (player != null)
                        {
                            _registeredPlayers.Remove(player);
                        }
                    }

                    break;
            }
            

        }

        public void LogoutPlayer()
        {
            lock (_lockerRegisteredPlayers)
            {
                var player = _registeredPlayers.First(x => x.ConnectionId == Context.ConnectionId);

                if (player != null)
                {
                    lock (_lockerMatchmaking)
                    {
                        _playersInMatchmaking.Remove(player);
                    }
                    _registeredPlayers.Remove(player);
                }
            }
        }

        public Task MatchEvents(Command cmd)
        {
            // reimplement when singleton match exists
            MatchEvents matchEvents = JsonConvert.DeserializeObject<MatchEvents>(cmd.Content);
            Match match = null;
            lock (_matches)
            {
                match = _matches.First(x => x.Players.Any(y => y.ConnectionId == Context.ConnectionId));

                if (match != null)
                {
                    match.Players.Remove(match.Players.First(x => x.ConnectionId == Context.ConnectionId));
                }
            }

            return Task.CompletedTask;
        }

        public async Task Location(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "MovePlayer":
                    Match match = null;
                    lock (_lockerMatches)
                    {

                        foreach (Match m in _matches)
                        {
                            foreach (Player p in m.Players)
                            {
                                if (p.Name.Equals(location.ShipName))
                                {
                                    match = m;
                                    break;
                                }
                            }
                        }
                    }
                    Player opponent = null;
                    if (match != null)
                        opponent = match.Players.FirstOrDefault(x => x.Name != location.ShipName);
                    if (opponent != null)
                    {
                        location.Response = "UpdateLocation";
                        Command answer = new Command("Location", JsonConvert.SerializeObject(location));
                        await SendAsync(answer, opponent.ConnectionId);
                    }
                    break;
                case "MoveBullet":
                    match = null;
                    lock (_lockerMatches)
                    {

                        foreach (Match m in _matches)
                        {
                            foreach (Player p in m.Players)
                            {
                                if (p.Name.Equals(location.ShipName))
                                {
                                    match = m;
                                    break;
                                }
                            }
                        }
                    }
                    opponent = null;
                    if (match != null)
                        opponent = match.Players.First(x => x.Name != location.ShipName);
                    if (opponent != null)
                    {
                        location.Response = "UpdateBulletLocation";
                        Command answer = new Command("Location", JsonConvert.SerializeObject(location));
                        await SendAsync(answer, opponent.ConnectionId);
                    }
                    break;
            }
            
        }

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
