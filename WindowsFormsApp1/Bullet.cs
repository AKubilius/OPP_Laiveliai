using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    internal class Bullet
    {
        public string direction; // creating a public string called direction
        public int speed = 20; // creating a integer called speed and assigning a value of 20
        PictureBox bullet = new PictureBox(); // create a picture box 
        Timer tm = new Timer(); // create a new timer called tm. 

        public int bulletLeft; // create a new public integer
        public int bulletTop; // create a new public integer

        public void mkBullet(Form form)
        {
            // this function will add the bullet to the game play
            // it is required to be called from the main class

            bullet.BackColor = System.Drawing.Color.Wheat; // set the colour white for the bullet
            bullet.Size = new Size(5, 5); // set the size to the bullet to 5 pixel by 5 pixel
            bullet.Tag = "bullet"; // set the tag to bullet
            bullet.Left = bulletLeft; // set bullet left 
            bullet.Top = bulletTop; // set bullet right
            bullet.BringToFront(); // bring the bullet to front of other objects
            form.Controls.Add(bullet); // add the bullet to the screen

            tm.Interval = speed; // set the timer interval to speed
            tm.Tick += new EventHandler(tm_Tick); // assignment the timer with an event
            tm.Start(); // start the timer
        }

        public void tm_Tick(object sender, EventArgs e)
        {
            // if direction equals to left
            if (direction == "left")
            {
                bullet.Left -= speed; // move bullet towards the left of the screen
            }
            // if direction equals right
            if (direction == "right")
            {
                bullet.Left += speed; // move bullet towards the right of the screen
            }
            // if direction is up
            if (direction == "up")
            {
                bullet.Top -= speed; // move the bullet towards top of the screen
            }
            // if direction is down
            if (direction == "down")
            {
                bullet.Top += speed; // move the bullet bottom of the screen
            }

            if (bullet.Left < 0 || bullet.Left > 1000 || bullet.Top < 0 || bullet.Top > 1000)
            {
                tm.Stop(); // stop the timer
                tm.Dispose(); // dispose the timer event and component from the program
                bullet.Dispose(); // dispose the bullet
                tm = null; // nullify the timer object
                bullet = null; // nullify the bullet object
            }
        }

    }
}
