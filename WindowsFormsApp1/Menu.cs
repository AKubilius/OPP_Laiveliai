using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Menu : Form
    {

        HubConnection _hubConnection;

        public Menu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7271/chat")
                .WithAutomaticReconnect()
                .Build();
        }

        private async void Play_CLick(object sender, EventArgs e)
        {
            this.Play.Enabled = false;
            this.Play.Text = "Loading";
            
            await _hubConnection.StartAsync();

            Map f = new Map(_hubConnection); // This is bad
            this.Hide(); // Hide the current form.

            f.Show(); // Show it
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
