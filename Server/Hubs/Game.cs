using ClassLib;
using ClassLib.Observer;
using ClassLib.Visitor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using Newtonsoft.Json;
using Server.Models;
using System.Numerics;
using System.Timers;
using static ClassLib.Command;

namespace Server.Hubs
{
    public class Game : Hub, ISubject
    {
        private static List<Player> _registeredPlayers = new List<Player>();
        private static Match _match = Match.Instance();
        private static List<Player> _playersInMatchmaking = new List<Player>();
        static private Random random = new Random();

        private object _lockerRegisteredPlayers = new object();
        private object _lockerMatchmaking = new object();
        private object _lockerMatch = new object();

        public void Subscribe(Player player)
        {
            lock (_lockerMatch)
            {
                _match.Players.Add(player);
                Console.WriteLine("Player joined the match! Name:" + player.Name + "| ID[" + player.ConnectionId + "] | Total players: " + _match.Players.Count);
            }
        }

        public void Unsubscribe(Player player)
        {
            lock (_lockerMatch)
            {
                _match.Players.Remove(player);
                Console.WriteLine("Player left the match! Name:" + player.Name + " | ID[" + player.ConnectionId + "] | Total players: " + _match.Players.Count);
            }
        }

        public void Accept(IVisitor visitor)
        {
            foreach(var player in _playersInMatchmaking)
            {
                player.Accept(visitor);
            }
        }

        public async Task Notify(string argument)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                MatchEvent events = new MatchEvent("MapEvent", argument);
                Command command = new Command("MatchEvent", JsonConvert.SerializeObject(events));
                foreach (var player in _match.Players)
                {
                    await SendAsync(command, player.ConnectionId);
                }
            }
            else
            {
                MatchEvent events = new MatchEvent("GlobalEvent", "");
                Command command = new Command("MatchEvent", JsonConvert.SerializeObject(events));
                foreach (var player in _match.Players)
                {
                    await SendAsync(command, player.ConnectionId);
                }
            }

        }


        public async void Message(Command cmd)
        {
            switch (cmd.Name)
            {
                case "Authentication":
                    await Authentication(cmd);
                    break;
                case "MatchEvent":
                    await Events(cmd);
                    break;
                case "Matchmaking":
                    await Matchmaking(cmd);
                    break;
                case "Location":
                    await Location(cmd);
                    break;
                case "Armour":
                    await Armours(cmd);
                    break;
            }
        }

        private Task StartTimer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;
            return Task.CompletedTask;
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var cmd = new Command("ChangeMap", "Red");
            await SendAllAsync(cmd);
        }

        public async Task SendAsync(Command cmd, string callerID)
        {
            await Clients.Client(callerID).SendAsync("Message", cmd);
        }

        private async Task SendAllAsync(Command cmd)
        {
            await Clients.All.SendAsync("Message", cmd);
        }

        private async Task SendAllExeptAsync(Command cmd, string connectionId)
        {
            await Clients.AllExcept(connectionId).SendAsync("Message", cmd);
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
                Player? player = _registeredPlayers.First(x => x.ConnectionId == Context.ConnectionId);

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

        public async Task Events(Command cmd)
        {
            MatchEvent events = JsonConvert.DeserializeObject<MatchEvent>(cmd.Content);

            switch (events.Response)
            {
                case "InitiateEvent":
                    await Notify(string.Empty);
                    break;
                case "MapEvent":
                    MatchEvent matchEvent = JsonConvert.DeserializeObject<MatchEvent>(cmd.Content);
                    await Notify(matchEvent.PlayerName);
                    break;
                case "LeftMatch":
                    var player = _match.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                    if (player != null)
                    {
                        Unsubscribe(player);
                        MatchEvent responseEvent = new MatchEvent("LeftMatch", player.Name);
                        Command cmdResponse = new Command("MatchEvent", JsonConvert.SerializeObject(responseEvent));
                        await SendAllAsync(cmdResponse);
                    }
                    break;
            }

            //// reimplement when singleton match exists
            //MatchEvents matchEvents = JsonConvert.DeserializeObject<MatchEvents>(cmd.Content);

            //switch (matchEvents.Response)
            //{
            //    case "LeftMatch":
            //        var player = _match.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            //        if (player != null)
            //        {
            //            _match.Players.Remove(_match.Players.First(x => x.ConnectionId == Context.ConnectionId));
            //            var cmdResponse = new Command("LeftMatch", player.Name);
            //            await SendAllAsync(cmdResponse);
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        public async Task Location(Command cmd)
        {
            Location location = JsonConvert.DeserializeObject<Location>(cmd.Content);

            switch (location.Response)
            {
                case "MovePlayer":
                    location.Response = "UpdateLocation";
                    Command answer = new Command("Location", JsonConvert.SerializeObject(location));
                    await SendAllExeptAsync(answer, Context.ConnectionId);
                    break;
                case "MoveBullet":
                    location.Response = "UpdateBulletLocation";
                    answer = new Command("Location", JsonConvert.SerializeObject(location));
                    await SendAllExeptAsync(answer, Context.ConnectionId);
                    break;
                case "MoveParticle":
                    Accept(new ParticleVisitor());
                    SendAsyncMessage(location);
                    break;
            }

        }

        public async Task Armours(Command cmd)
        {
            await SendAllExeptAsync(cmd, Context.ConnectionId);
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

        public async void SendAsyncMessage(Location location)
        {
            location.Response = "UpdateParticleLocation";
            Command answer = new Command("Location", JsonConvert.SerializeObject(location));
            await SendAllAsync(answer);
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
            }

            if (_match.Players.Count == 10)
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

            Subscribe(player);

            matchmaking.Response = "MatchCreated";

            matchmaking.StartingID = 0;
            matchmaking.StartingYPos = random.Next(0, 60);
            answer = new Command("Matchmaking", JsonConvert.SerializeObject(matchmaking));
            await SendAsync(answer, player.ConnectionId);
        }


    }
}
