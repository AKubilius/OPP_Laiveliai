using ClassLib.Strategy;
using ClassLib.Units.Bullet;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Forms;

namespace ClassLib.Units.Ship
{
    public class Ship : Unit
    {
        public ProgressBar Health { get; set; }

        public int Speed { get; set; }
        public int Power { get; set; }

        public IStrategy Strategy { get; set; }

        public Ship(string shipName)
        {
            this.Health = new ProgressBar();
            this.Health.Name = "Health";
            this.Health.Size = new Size(40, 6);
            this.Health.TabIndex = 0;
            this.Health.Value = Convert.ToInt32(100);
            this.Image = new PictureBox();
            this.Image.BackColor = System.Drawing.Color.Transparent;
            this.Image.Name = shipName;
            this.Image.Size = new System.Drawing.Size(56, 36);
            this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image.TabIndex = 0;
            this.Image.TabStop = false;
            this.Label = new Label();
            this.Label.AutoSize = true;
            this.Label.Name = "label" + shipName;
            this.Label.Size = new System.Drawing.Size(35, 13);
            this.Label.TabIndex = 0;
            this.Label.Text = "label2";
            this.Label.BorderStyle = BorderStyle.FixedSingle;
            this.Label.ForeColor = System.Drawing.Color.White;
        }

        public Ship() 
        {

        }

        public void SetStrategy(IStrategy strategy)
        {
            this.Strategy = strategy;
        }

        public IStrategy GetStrategy()
        {
            return this.Strategy;
        }

        public override void Dispose()
        {
            base.Dispose();
            Health.Dispose();
        }

        public void Shoot(object weapon, Form form, string playerName, HubConnection hubConnection, int bulletId)
        {
            Strategy.DoAlgorithm(weapon, form, playerName, hubConnection, bulletId);
        }
    }
}
