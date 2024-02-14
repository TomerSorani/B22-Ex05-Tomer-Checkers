namespace CheckersUserInterface
{
    partial class CheckersGameSettings
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
            this.checkBoxSecondPlayer = new System.Windows.Forms.CheckBox();
            this.buttonDoneSettings = new System.Windows.Forms.Button();
            this.textBoxSecondPlayerName = new System.Windows.Forms.TextBox();
            this.textBoxFirstPlayerName = new System.Windows.Forms.TextBox();
            this.radioButton6x6 = new System.Windows.Forms.RadioButton();
            this.radioButton8x8 = new System.Windows.Forms.RadioButton();
            this.radioButton10x10 = new System.Windows.Forms.RadioButton();
            this.labelBoardSize = new System.Windows.Forms.Label();
            this.labelPlayers = new System.Windows.Forms.Label();
            this.labelFirstPlayer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxSecondPlayer
            // 
            this.checkBoxSecondPlayer.AutoSize = true;
            this.checkBoxSecondPlayer.Location = new System.Drawing.Point(46, 188);
            this.checkBoxSecondPlayer.Name = "checkBoxSecondPlayer";
            this.checkBoxSecondPlayer.Size = new System.Drawing.Size(82, 21);
            this.checkBoxSecondPlayer.TabIndex = 0;
            this.checkBoxSecondPlayer.Text = "Player2:";
            this.checkBoxSecondPlayer.UseVisualStyleBackColor = true;
            this.checkBoxSecondPlayer.CheckedChanged += new System.EventHandler(this.checkBoxSecondPlayer_CheckedChanged);
            // 
            // buttonDoneSettings
            // 
            this.buttonDoneSettings.Location = new System.Drawing.Point(288, 259);
            this.buttonDoneSettings.Name = "buttonDoneSettings";
            this.buttonDoneSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonDoneSettings.TabIndex = 1;
            this.buttonDoneSettings.Text = "Done";
            this.buttonDoneSettings.UseVisualStyleBackColor = true;
            this.buttonDoneSettings.Click += new System.EventHandler(this.buttonDoneSettings_Click);
            // 
            // textBoxSecondPlayerName
            // 
            this.textBoxSecondPlayerName.Enabled = false;
            this.textBoxSecondPlayerName.Location = new System.Drawing.Point(188, 188);
            this.textBoxSecondPlayerName.Name = "textBoxSecondPlayerName";
            this.textBoxSecondPlayerName.Size = new System.Drawing.Size(100, 22);
            this.textBoxSecondPlayerName.TabIndex = 2;
            this.textBoxSecondPlayerName.Text = "Computer";
            // 
            // textBoxFirstPlayerName
            // 
            this.textBoxFirstPlayerName.Location = new System.Drawing.Point(188, 127);
            this.textBoxFirstPlayerName.Name = "textBoxFirstPlayerName";
            this.textBoxFirstPlayerName.Size = new System.Drawing.Size(100, 22);
            this.textBoxFirstPlayerName.TabIndex = 3;
            // 
            // radioButton6x6
            // 
            this.radioButton6x6.AutoSize = true;
            this.radioButton6x6.Location = new System.Drawing.Point(41, 40);
            this.radioButton6x6.Name = "radioButton6x6";
            this.radioButton6x6.Size = new System.Drawing.Size(62, 21);
            this.radioButton6x6.TabIndex = 4;
            this.radioButton6x6.TabStop = true;
            this.radioButton6x6.Text = "6 X 6";
            this.radioButton6x6.UseVisualStyleBackColor = true;
            this.radioButton6x6.CheckedChanged += new System.EventHandler(this.radioButton6x6_CheckedChanged);
            // 
            // radioButton8x8
            // 
            this.radioButton8x8.AutoSize = true;
            this.radioButton8x8.Location = new System.Drawing.Point(164, 40);
            this.radioButton8x8.Name = "radioButton8x8";
            this.radioButton8x8.Size = new System.Drawing.Size(62, 21);
            this.radioButton8x8.TabIndex = 5;
            this.radioButton8x8.TabStop = true;
            this.radioButton8x8.Text = "8 X 8";
            this.radioButton8x8.UseVisualStyleBackColor = true;
            this.radioButton8x8.CheckedChanged += new System.EventHandler(this.radioButton8x8_CheckedChanged);
            // 
            // radioButton10x10
            // 
            this.radioButton10x10.AutoSize = true;
            this.radioButton10x10.Location = new System.Drawing.Point(285, 40);
            this.radioButton10x10.Name = "radioButton10x10";
            this.radioButton10x10.Size = new System.Drawing.Size(78, 21);
            this.radioButton10x10.TabIndex = 6;
            this.radioButton10x10.TabStop = true;
            this.radioButton10x10.Text = "10 X 10";
            this.radioButton10x10.UseVisualStyleBackColor = true;
            this.radioButton10x10.CheckedChanged += new System.EventHandler(this.radioButton10x10_CheckedChanged);
            // 
            // labelBoardSize
            // 
            this.labelBoardSize.AutoSize = true;
            this.labelBoardSize.Location = new System.Drawing.Point(22, 9);
            this.labelBoardSize.Name = "labelBoardSize";
            this.labelBoardSize.Size = new System.Drawing.Size(81, 17);
            this.labelBoardSize.TabIndex = 7;
            this.labelBoardSize.Text = "Board Size:";
            // 
            // labelPlayers
            // 
            this.labelPlayers.AutoSize = true;
            this.labelPlayers.Location = new System.Drawing.Point(22, 79);
            this.labelPlayers.Name = "labelPlayers";
            this.labelPlayers.Size = new System.Drawing.Size(59, 17);
            this.labelPlayers.TabIndex = 8;
            this.labelPlayers.Text = "Players:";
            // 
            // labelFirstPlayer
            // 
            this.labelFirstPlayer.AutoSize = true;
            this.labelFirstPlayer.Location = new System.Drawing.Point(43, 127);
            this.labelFirstPlayer.Name = "labelFirstPlayer";
            this.labelFirstPlayer.Size = new System.Drawing.Size(60, 17);
            this.labelFirstPlayer.TabIndex = 9;
            this.labelFirstPlayer.Text = "Player1:";
            // 
            // GameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(392, 302);
            this.Controls.Add(this.labelFirstPlayer);
            this.Controls.Add(this.labelPlayers);
            this.Controls.Add(this.labelBoardSize);
            this.Controls.Add(this.radioButton10x10);
            this.Controls.Add(this.radioButton8x8);
            this.Controls.Add(this.radioButton6x6);
            this.Controls.Add(this.textBoxFirstPlayerName);
            this.Controls.Add(this.textBoxSecondPlayerName);
            this.Controls.Add(this.buttonDoneSettings);
            this.Controls.Add(this.checkBoxSecondPlayer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckersGameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxSecondPlayer;
        private System.Windows.Forms.Button buttonDoneSettings;
        private System.Windows.Forms.TextBox textBoxSecondPlayerName;
        private System.Windows.Forms.TextBox textBoxFirstPlayerName;
        private System.Windows.Forms.RadioButton radioButton6x6;
        private System.Windows.Forms.RadioButton radioButton8x8;
        private System.Windows.Forms.RadioButton radioButton10x10;
        private System.Windows.Forms.Label labelBoardSize;
        private System.Windows.Forms.Label labelPlayers;
        private System.Windows.Forms.Label labelFirstPlayer;
    }
}

