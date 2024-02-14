
namespace CheckersUserInterface
{
    partial class CheckersUi
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
            this.labelCurrentTurn = new System.Windows.Forms.Label();
            this.labelTurn = new System.Windows.Forms.Label();
            this.labelPlayer1Points = new System.Windows.Forms.Label();
            this.labelPlayer2Points = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCurrentTurn
            // 
            this.labelCurrentTurn.AutoSize = true;
            this.labelCurrentTurn.Location = new System.Drawing.Point(42, 50);
            this.labelCurrentTurn.Name = "labelCurrentTurn";
            this.labelCurrentTurn.Size = new System.Drawing.Size(95, 20);
            this.labelCurrentTurn.TabIndex = 0;
            this.labelCurrentTurn.Text = "current turn:";
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(143, 50);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(64, 20);
            this.labelTurn.TabIndex = 2;
            this.labelTurn.Text = "player 1";
            // 
            // labelPlayer1Points
            // 
            this.labelPlayer1Points.AutoSize = true;
            this.labelPlayer1Points.Location = new System.Drawing.Point(56, 112);
            this.labelPlayer1Points.Name = "labelPlayer1Points";
            this.labelPlayer1Points.Size = new System.Drawing.Size(61, 20);
            this.labelPlayer1Points.TabIndex = 3;
            this.labelPlayer1Points.Text = "Player1";
            // 
            // labelPlayer2Points
            // 
            this.labelPlayer2Points.AutoSize = true;
            this.labelPlayer2Points.Location = new System.Drawing.Point(56, 156);
            this.labelPlayer2Points.Name = "labelPlayer2Points";
            this.labelPlayer2Points.Size = new System.Drawing.Size(61, 20);
            this.labelPlayer2Points.TabIndex = 4;
            this.labelPlayer2Points.Text = "Player2";
            // 
            // labelPoints
            // 
            this.labelPoints.AutoSize = true;
            this.labelPoints.Location = new System.Drawing.Point(56, 82);
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(57, 20);
            this.labelPoints.TabIndex = 5;
            this.labelPoints.Text = "Points:";
            // 
            // CheckersUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::CheckersUserInterface.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1105, 875);
            this.Controls.Add(this.labelPoints);
            this.Controls.Add(this.labelPlayer2Points);
            this.Controls.Add(this.labelPlayer1Points);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.labelCurrentTurn);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "CheckersUi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checkers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentTurn;
        private System.Windows.Forms.Label labelTurn;
        private System.Windows.Forms.Label labelPlayer1Points;
        private System.Windows.Forms.Label labelPlayer2Points;
        private System.Windows.Forms.Label labelPoints;
    }
}