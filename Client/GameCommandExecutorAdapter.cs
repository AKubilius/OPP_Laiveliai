using ClassLib;
using Newtonsoft.Json;
using static ClassLib.Command;

namespace Client
{
    internal class GameCommandExecutorAdapter : CommandExecutor
    {
        private readonly Game _game;

        public GameCommandExecutorAdapter(Game game)
        {
            _game = game;
        }

        public override Task Execute(Command command)
        {
            switch (command.Name)
            {
                case "Register":
                    RegisterPlayer(command);
                    break;
                case "Matchmaking":
                    Matchmaking(command);
                    break;
            }
            return Task.CompletedTask;
        }

        private void RegisterPlayer(Command cmd)
        {
            Authentication auth = JsonConvert.DeserializeObject<Authentication>(cmd.Content);

            switch (auth.Response)
            {
                case "RegisterComplete":
                    _game.RegisterComplete();
                    break;

                case "NameOccupied":
                    _game.NameOccupied();
                    break;
            }
        }

        private void Matchmaking(Command cmd)
        {
            Matchmaking matchmaking = JsonConvert.DeserializeObject<Matchmaking>(cmd.Content);

            switch (matchmaking.Response)
            {
                case "InMatchmakingQueue":
                    _game.InMatchMakingQueue();
                    break;

                case "MatchCreated":
                    _game.MatchCreated(matchmaking);
                    break;
            }
        }

    }
}
