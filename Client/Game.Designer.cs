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
            this.SuspendLayout();
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(411, 140);
            this.Register.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(88, 27);
            this.Register.TabIndex = 0;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            this.Register.Click += new System.EventHandler(this.Register_click);
            // 
            // PlayerName
            // 
            this.PlayerName.Location = new System.Drawing.Point(369, 113);
            this.PlayerName.Margin = new System.Windows.Forms.Padding(2);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(176, 23);
            this.PlayerName.TabIndex = 1;
            // 
            // Play
            // 
            this.Play.Enabled = false;
            this.Play.Location = new System.Drawing.Point(421, 172);
            this.Play.Margin = new System.Windows.Forms.Padding(2);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(65, 32);
            this.Play.TabIndex = 2;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Visible = false;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // LobbyStatus
            // 
            this.LobbyStatus.AutoSize = true;
            this.LobbyStatus.Location = new System.Drawing.Point(384, 207);
            this.LobbyStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LobbyStatus.Name = "LobbyStatus";
            this.LobbyStatus.Size = new System.Drawing.Size(141, 15);
            this.LobbyStatus.TabIndex = 3;
            this.LobbyStatus.Text = "Searching for opponent...";
            this.LobbyStatus.Visible = false;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(436, 92);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 15);
            this.infoLabel.TabIndex = 4;
            // 
            // gameTitle
            // 
            this.gameTitle.AutoSize = true;
            this.gameTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gameTitle.Location = new System.Drawing.Point(359, 21);
            this.gameTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gameTitle.Name = "gameTitle";
            this.gameTitle.Size = new System.Drawing.Size(167, 45);
            this.gameTitle.TabIndex = 5;
            this.gameTitle.Text = "LAIVELIAI";
            // 
            // loggedInAs
            // 
            this.loggedInAs.AutoSize = true;
            this.loggedInAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.loggedInAs.Location = new System.Drawing.Point(400, 77);
            this.loggedInAs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggedInAs.Name = "loggedInAs";
            this.loggedInAs.Size = new System.Drawing.Size(88, 13);
            this.loggedInAs.TabIndex = 6;
            this.loggedInAs.Text = "Logged in as: ";
            this.loggedInAs.Visible = false;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.loggedInAs);
            this.Controls.Add(this.gameTitle);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.LobbyStatus);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.PlayerName);
            this.Controls.Add(this.Register);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Game";
            this.Text = "Game";
            this.Load += new System.EventHandler(this.Menu_Load);
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
    }
}