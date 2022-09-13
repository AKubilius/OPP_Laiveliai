using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace WindowsFormsApp1
{
    public partial class Map : Form
    {
        HubConnection hubConnection;
        
        public Map()
        {
            InitializeComponent();

        }

        bool moveRight, moveLeft,moveUp,moveDown;
        int speed = 10;
        string facing = "up";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void moveTimerEvent(object sender, EventArgs e)
        {
            if (moveLeft && Ship.Left > 0)
            {
                Ship.Left -= speed;
            }

            if (moveRight && Ship.Right < 1000)
            {
                Ship.Left += speed;
            }

            if (moveUp && Ship.Top > 0)
            {
                Ship.Top -= speed;
            }
            if (moveDown && Ship.Top < 1000)
            {
                Ship.Top += speed;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                facing = "left";
                Ship.Image = Properties.Resources.left;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                facing = "right";
                Ship.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = true;
                facing = "down";
                Ship.Image = Properties.Resources.down;
            }
            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;
                facing = "up";
                Ship.Image = Properties.Resources.up;
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
            shoot.bulletLeft = Ship.Left + (Ship.Width / 2); // place the bullet to left half of the player
            shoot.bulletTop = Ship.Top + (Ship.Height / 2); // place the bullet on top half of the player
            shoot.mkBullet(this); // run the function mkbullet from the bullet class. 
        }

      
    }
}
