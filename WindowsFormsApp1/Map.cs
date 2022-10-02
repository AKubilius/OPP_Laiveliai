using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{
    public partial class Map : Form
    {
        HubConnection _hubConnection;
        int _matchId;
        string _playerName;
        Ship player;
        Ship opponent;
        Game _mainMenu;
        bool _isForcedToLeave;


        //next level  
        private int x()
        {
            return Process.GetCurrentProcess().Id / 40;
        }
        private int y()
        {
            return Process.GetCurrentProcess().Id / 40;
        }
        //


        public Map(HubConnection hubConnection, int matchId, string playerName, Game mainMenu)
        {
            this.FormClosing += new FormClosingEventHandler(Map_Closing);
            _isForcedToLeave = false;
            _mainMenu = mainMenu;

            InitializeComponent(2);
           
            player = ships[0];
            player.ShipLabel.Text = playerName;
            player.ShipLabel.Location = new Point(x(), y() - 50);
            player.ShipImage.Location = new Point(x(), y());
            this._matchId = matchId;
            this._playerName = playerName;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;


            _hubConnection = hubConnection;

            // for now its 1v1, so ships[1] is opponent and ships[0] is player

            _hubConnection.On<string, string, int, int>("LocationInfo", (shipName, facing, xAxis, yAxis) =>
            {
                opponent = ships[1];
                opponent.ShipImage.Location = new Point(xAxis, yAxis);
                opponent.ShipLabel.Location = new Point(xAxis, yAxis - 50);
                opponent.ShipLabel.Text = shipName;
                opponent.ShipImage.Image = facing.Equals("right") ? Properties.Resources.right : Properties.Resources.left;
            });

            _hubConnection.On("LeaveMatch", () =>
            {
                _isForcedToLeave = true;
                this.Close();
            });
        }

        bool moveRight, moveLeft, moveUp, moveDown;
        int speed = 10;
        string facing = "up";


        private async void moveTimerEvent(object sender, EventArgs e)
        {
            if (moveLeft && player.ShipImage.Left > 0)
            {
                player.ShipImage.Left -= speed;
                player.ShipLabel.Left -= speed;
            }

            if (moveRight && player.ShipImage.Right < 700)
            {
                player.ShipImage.Left += speed;
                player.ShipLabel.Left += speed;
            }

            if (moveUp && player.ShipImage.Top > 0)
            {
                player.ShipImage.Top -= speed;
                player.ShipLabel.Top -= speed;
            }
            if (moveDown && player.ShipImage.Top < 700)
            {
                player.ShipImage.Top += speed;
                player.ShipLabel.Top += speed;
            }

            await _hubConnection.SendAsync("SendLocation", _matchId, _playerName, facing, player.ShipImage.Location.X, player.ShipImage.Location.Y);

        }
        private async void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                facing = "left";
                player.ShipImage.Image = Properties.Resources.left;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                facing = "right";
                player.ShipImage.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = true;
                facing = "down";
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
                facing = "up";
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                shoot(facing);
            }

        }
        private void shoot(string direct)
        {
            // this is the function thats makes the new bullets in this game

            Bullet shoot = new Bullet(); // create a new instance of the bullet class
            shoot.direction = direct; // assignment the direction to the bullet
            shoot.bulletLeft = player.ShipImage.Left + (player.ShipImage.Width / 2); // place the bullet to left half of the player
            shoot.bulletTop = player.ShipImage.Top + (player.ShipImage.Height / 2); // place the bullet on top half of the player
            shoot.mkBullet(this); // run the function mkbullet from the bullet class. 
        }

        private async void Map_Closing(object sender, CancelEventArgs e)
        {
            if (!_isForcedToLeave)
            {
                await _hubConnection.SendAsync("LeftMatch");
            }

            _mainMenu.Visible = true;
        }
    }
}