using ClassLib.Units.Ship;

namespace Client
{
    partial class Map
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(int shipCount)
        {
            this.components = new System.ComponentModel.Container();
            this.moveTimer = new System.Windows.Forms.Timer(this.components);
            this.background = new System.Windows.Forms.PictureBox();
            for (int i = 0; i< shipCount; i++)
            {
                Ship ship = new Ship("ship" + i);
                ships.Add(ship);
                ((System.ComponentModel.ISupportInitialize)(ship.Image)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.background)).BeginInit();
                this.Controls.Add(ship.Image);
                this.Controls.Add(ship.Label);
                this.Controls.Add(this.background);
                ((System.ComponentModel.ISupportInitialize)(ship.Image)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.background)).EndInit();

            }
            this.SuspendLayout();
            // 
            // moveTimer
            // 
            this.moveTimer.Enabled = true;
            this.moveTimer.Interval = 20;
            this.moveTimer.Tick += new System.EventHandler(this.moveTimerEvent);
            //Background picturebox
            this.background.BackgroundImage = global::Client.Properties.Resources.background;
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(1500, 1000);
            this.background.TabIndex = 1;
            this.background.TabStop = false;
            
// 
            // Map
            // 

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(700, 700);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Location = new System.Drawing.Point(500, 500);
            this.Name = "Map";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map";
            this.TopMost = true;
           // this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyisdown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyisup);
            this.ResumeLayout(false);

        }

        #endregion

        private List<Ship> ships = new List<Ship>();
        private System.Windows.Forms.Timer moveTimer;
        private System.Windows.Forms.PictureBox background;
    }
}

