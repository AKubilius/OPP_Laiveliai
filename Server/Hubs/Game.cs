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
        private static Match _match = Match.Instance();
        private static List<Player> _playersInMatchmaking = new List<Player>();
        static private Random random = new Random();

        private object _lockerRegisteredPlayers = new object();
        private object _lockerMatchmaking = new object();


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

        private async Task SendAllAsync(Command cmd)
        {
            await Clients.All.SendAsync("Message", cmd);
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

            _match.Players.Remove(_match.Players.First(x => x.ConnectionId == Context.ConnectionId));

            return Task.CompletedTask;
        }

        public async Task Location(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "MovePlayer":
                    location.Response = "UpdateLocation";
                    Command answer = new Command("Location", JsonConvert.SerializeObject(location));
                    await SendAllAsync(answer);
                    break;
                case "MoveBullet":
                    location.Response = "UpdateBulletLocation";
                    answer = new Command("Location", JsonConvert.SerializeObject(location));
                    await SendAllAsync(answer);
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

        public async Task JoinMatchmaking(string ConnectionId)
        {
            Matchmaking matchmaking = new Matchmaking();
            Command answer = null;

            var player = _registeredPlayers.FirstOrDefault(x => x.ConnectionId == ConnectionId);
            if (player == null) return;

            lock (_lockerMatchmaking)
            {
                _playersInMatchmaking.Add(player);
                //opponent = _playersInMatchmaking.FirstOrDefault(x => x.ConnectionId != player.ConnectionId);
            }

            if (_match.Players.Count == 3)
            {
                matchmaking.Response = "InMatchmakingQueue";
                answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
                await SendAsync(answer, player.ConnectionId);

                return;
            }

            lock (_lockerMatchmaking)
            {
                _playersInMatchmaking.Remove(player);
            }

            _match.Players.Add(player);

            matchmaking.Response = "MatchCreated";

            matchmaking.StartingID = 0;
            matchmaking.StartingYPos = random.Next(0, 60);
            answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
            await SendAsync(answer, player.ConnectionId);
        }
    }
}
