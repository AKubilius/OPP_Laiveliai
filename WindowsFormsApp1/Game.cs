using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class Game : Form
    {

        HubConnection _hubConnection;

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

            _hubConnection.On("RegisterComplete", () =>
            {
                this.Register.Visible = false;
                this.PlayerName.Visible = false;

                this.loggedInAs.Text = "Logged in as: " + PlayerName.Text;
                this.loggedInAs.Visible = true;


                this.Play.Visible = true;
                this.Play.Enabled = true;
            });

            _hubConnection.On("InMatchmakingQueue", () =>
            {
                this.LobbyStatus.Visible = true;
                this.LobbyStatus.Text = "Searching for opponent...";
            });

            _hubConnection.On<string>("FoundOpponent", (name) =>
            {

                this.LobbyStatus.Visible = true;
                this.LobbyStatus.Text = $"Found opponent: {name}";
            });

            _hubConnection.On<int, int, int>("MatchCreated", (id, startingId, randomY) =>
            {
                Map map = new Map(_hubConnection, id, PlayerName.Text, startingId, randomY, this);
                this.LobbyStatus.Visible = false;
                this.Play.Enabled = true;
                this.Hide();
                map.Show();
            });


            _hubConnection.On("NameOccupied", () =>
            {
                this.infoLabel.Visible = true;
                this.infoLabel.Text = "Name already exists!";
                this.PlayerName.Enabled = true;
                this.Register.Enabled = true;
            });
        }

        private async void Register_click(object sender, EventArgs e)
        {
            this.infoLabel.Visible = false;
            this.PlayerName.Enabled = false;
            this.Register.Enabled = false;

            await _hubConnection.SendAsync("RegisterPlayer", PlayerName.Text);
        }

        private async void Menu_Load(object sender, EventArgs e)
        {
            await _hubConnection.StartAsync();
        }

        private async void Play_Click(object sender, EventArgs e)
        {
            this.Play.Enabled = false;

            await _hubConnection.SendAsync("FindOpponent");
        }

        private async void Game_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await _hubConnection.SendAsync("LogoutPlayer");
        }
    }
}