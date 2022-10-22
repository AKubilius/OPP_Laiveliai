using System.Windows.Forms;

namespace Client
{
    partial class Game
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
        private void InitializeComponent()
        {
            this.Register = new System.Windows.Forms.Button();
            this.PlayerName = new System.Windows.Forms.TextBox();
            this.Play = new System.Windows.Forms.Button();
            this.LobbyStatus = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.gameTitle = new System.Windows.Forms.Label();
            this.loggedInAs = new System.Windows.Forms.Label();
            this.SkinGroupBox = new System.Windows.Forms.GroupBox();
            this.Red = new System.Windows.Forms.RadioButton();
            this.White = new System.Windows.Forms.RadioButton();
            this.SkinGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(470, 187);
            this.Register.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(101, 36);
            this.Register.TabIndex = 0;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            this.Register.Click += new System.EventHandler(this.Register_click);
            // 
            // PlayerName
            // 
            this.PlayerName.Location = new System.Drawing.Point(422, 151);
            this.PlayerName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(201, 27);
            this.PlayerName.TabIndex = 1;
            // 
            // Play
            // 
            this.Play.Enabled = false;
            this.Play.Location = new System.Drawing.Point(481, 229);
            this.Play.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(74, 43);
            this.Play.TabIndex = 2;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Visible = false;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // LobbyStatus
            // 
            this.LobbyStatus.AutoSize = true;
            this.LobbyStatus.Location = new System.Drawing.Point(439, 276);
            this.LobbyStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LobbyStatus.Name = "LobbyStatus";
            this.LobbyStatus.Size = new System.Drawing.Size(175, 20);
            this.LobbyStatus.TabIndex = 3;
            this.LobbyStatus.Text = "Searching for opponent...";
            this.LobbyStatus.Visible = false;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(498, 123);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 20);
            this.infoLabel.TabIndex = 4;
            // 
            // gameTitle
            // 
            this.gameTitle.AutoSize = true;
            this.gameTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gameTitle.Location = new System.Drawing.Point(410, 28);
            this.gameTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.gameTitle.Name = "gameTitle";
            this.gameTitle.Size = new System.Drawing.Size(207, 54);
            this.gameTitle.TabIndex = 5;
            this.gameTitle.Text = "LAIVELIAI";
            // 
            // loggedInAs
            // 
            this.loggedInAs.AutoSize = true;
            this.loggedInAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.loggedInAs.Location = new System.Drawing.Point(457, 103);
            this.loggedInAs.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.loggedInAs.Name = "loggedInAs";
            this.loggedInAs.Size = new System.Drawing.Size(112, 17);
            this.loggedInAs.TabIndex = 6;
            this.loggedInAs.Text = "Logged in as: ";
            this.loggedInAs.Visible = false;
            // 
            // SkinGroupBox
            // 
            this.SkinGroupBox.Controls.Add(this.Red);
            this.SkinGroupBox.Controls.Add(this.White);
            this.SkinGroupBox.Location = new System.Drawing.Point(389, 310);
            this.SkinGroupBox.Name = "SkinGroupBox";
            this.SkinGroupBox.Size = new System.Drawing.Size(250, 96);
            this.SkinGroupBox.TabIndex = 7;
            this.SkinGroupBox.TabStop = false;
            this.SkinGroupBox.Text = "Choose Skin:";
            // 
            // Red
            // 
            this.Red.AutoSize = true;
            this.Red.Location = new System.Drawing.Point(21, 56);
            this.Red.Name = "Red";
            this.Red.Size = new System.Drawing.Size(56, 24);
            this.Red.TabIndex = 8;
            this.Red.Text = "Red";
            this.Red.UseVisualStyleBackColor = true;
            this.Red.CheckedChanged += new System.EventHandler(this.Red_CheckedChanged);
            // 
            // White
            // 
            this.White.AutoSize = true;
            this.White.Checked = true;
            this.White.Location = new System.Drawing.Point(21, 26);
            this.White.Name = "White";
            this.White.Size = new System.Drawing.Size(69, 24);
            this.White.TabIndex = 8;
            this.White.TabStop = true;
            this.White.Text = "White";
            this.White.UseVisualStyleBackColor = true;
            this.White.CheckedChanged += new System.EventHandler(this.White_CheckedChanged);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 692);
            this.Controls.Add(this.SkinGroupBox);
            this.Controls.Add(this.loggedInAs);
            this.Controls.Add(this.gameTitle);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.LobbyStatus);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.Register);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "Game";
            this.Text = "Game";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.SkinGroupBox.ResumeLayout(false);
            this.SkinGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Register;
        private TextBox PlayerName;
        private Button Play;
        private Label LobbyStatus;
        private Label infoLabel;
        private Label gameTitle;
        private Label loggedInAs;
        private GroupBox SkinGroupBox;
        private RadioButton White;
        private RadioButton Red;
    }
}