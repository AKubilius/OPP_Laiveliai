using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows.Forms;

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

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7057/game")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("RegisterComplete", () =>
            {
                this.Register.Text = "Registered";
            });

            _hubConnection.On("RegisterComplete", () =>
            {
                this.Register.Text = "Registered";

                this.Play.Visible = true;
                this.Play.Enabled = true;
            });

            _hubConnection.On("NoOpponents", () =>
            {
                this.LobbyStatus.Visible = true;
                this.LobbyStatus.Text = "Searching for opponent...";
            });

            _hubConnection.On<string>("FoundOpponent", (name) =>
            {

                this.LobbyStatus.Visible = true;
                this.LobbyStatus.Text = $"Found opponent: {name}";
            });

            _hubConnection.On<int>("MatchCreated", (id) =>
            {
                Map map = new Map(_hubConnection, id, PlayerName.Text);
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
    }
}