using System.Windows.Forms;

namespace ClassLib.Units.Ship
{
    public class Ship : Unit
    {
        public ProgressBar Health { get; set; }

        public Ship(string shipName)
        {
            this.Image = new PictureBox();
            this.Image.BackColor = System.Drawing.Color.Transparent;
            this.Health = new ProgressBar();
            this.Health.Name = "Health";
            this.Health.Size = new Size(40, 6);
            this.Health.TabIndex = 0;
            this.Health.Value = Convert.ToInt32(100);
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

        public override void Dispose()
        {
            base.Dispose();
            Health.Dispose();
        }
    }
}
