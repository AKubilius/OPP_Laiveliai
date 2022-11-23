using ClassLib;
using ClassLib.Units.Ship;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using static ClassLib.Command;

namespace Client
{
    public partial class Game : Form
    {

        public HubConnection _hubConnection;
        Skin _skin;
        CommandExecutor _commandExecutor;

        public Game()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += new FormClosingEventHandler(Game_Closing);

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7150/game")
                .WithAutomaticReconnect()
                .Build();
            
            _hubConnection.ServerTimeout = TimeSpan.FromMinutes(30);

            _hubConnection.HandshakeTimeout = TimeSpan.FromMinutes(30);

            _commandExecutor = new GameCommandExecutorAdapter(this);

            _hubConnection.On<Command>("Message", (cmd) =>
            {
                _commandExecutor.Execute(cmd);
            });
        }

        public Skin GetSkin() => _skin;

        protected internal void NameOccupied()
        {
            this.infoLabel.Visible = true;
            this.infoLabel.Text = "Name already exists!";
            this.PlayerName.Enabled = true;
            this.Register.Enabled = true;
        }

        protected internal void RegisterComplete()
        {
            this.Register.Visible = false;
            this.PlayerName.Visible = false;

            this.loggedInAs.Text = "Logged in as: " + PlayerName.Text;
            this.loggedInAs.Visible = true;


            this.Play.Visible = true;
            this.Play.Enabled = true;
        }

        protected internal void MatchCreated(Matchmaking matchmaking)
        {
            Map map = new Map(_hubConnection, PlayerName.Text, matchmaking.StartingID, matchmaking.StartingYPos, this);
            this.LobbyStatus.Visible = false;
            this.Play.Enabled = true;
            this.Hide();
            map.Show();
        }

        protected internal void InMatchMakingQueue()
        {
            this.LobbyStatus.Visible = true;
            this.LobbyStatus.Text = "Searching for opponent...";
        }

        private async Task SendAsync(Command cmd)
        {
            await _hubConnection.SendAsync("Message", cmd);
        }

        private async void Register_click(object sender, EventArgs e)
        {
            this.infoLabel.Visible = false;
            this.PlayerName.Enabled = false;
            this.Register.Enabled = false;

            Authentication auth = new Authentication(PlayerName.Text, "Register");
            Command cmd = new Command("Authentication", JsonConvert.SerializeObject(auth));
            await SendAsync(cmd);
        }

        private async void Menu_Load(object sender, EventArgs e)
        {
            await _hubConnection.StartAsync();
        }

        private async void Play_Click(object sender, EventArgs e)
        {
            this.Play.Enabled = false;

            Matchmaking register = new Matchmaking("JoinMatchmaking", -1, -1);
            Command cmd = new Command("Matchmaking", JsonConvert.SerializeObject(register));
            await SendAsync(cmd);
        }

        private async void Game_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await _hubConnection.SendAsync("LogoutPlayer");
        }

        private void White_CheckedChanged(object sender, EventArgs e)
        {
            if (White.Checked)
            {
                _skin = Skin.White;
            }
        }

        private void Red_CheckedChanged(object sender, EventArgs e)
        {
            if (Red.Checked)
            {
                _skin = Skin.Red;
            }
        }

        private void Blue_CheckedChanged(object sender, EventArgs e)
        {
            if (Blue.Checked)
            {
                _skin = Skin.Blue;
            }
        }

        private void Yellow_CheckedChanged(object sender, EventArgs e)
        {
            if (Yellow.Checked)
            {
                _skin = Skin.Yellow;
            }
        }
    }
}