using System;
using System.Windows.Forms;
using CheckersEngine.Enums;

namespace CheckersUserInterface
{
    public partial class CheckersGameSettings : Form
    {
        private eCheckersBoardSize m_BoardSize = eCheckersBoardSize.SmallSize;

        public string FirstPlayerName
        {
            get
            {
                return textBoxFirstPlayerName.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return textBoxSecondPlayerName.Text;
            }
        }

        public eCheckersBoardSize BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        public eGameMode GameMode
        {
            get
            {
                return checkBoxSecondPlayer.Checked
                           ? eGameMode.PlayAgainstAnotherPlayerMode
                           : eGameMode.PlayAgainstTheComputerMode;
            }
        }

        public CheckersGameSettings()
        {
            InitializeComponent();
        }

        private void buttonDoneSettings_Click(object i_Sender, EventArgs i_EventArguments)
        {
            if (textBoxFirstPlayerName.Text != string.Empty && textBoxSecondPlayerName.Text != string.Empty
                                                           && (radioButton6x6.Checked || radioButton8x8.Checked
                                                               || radioButton10x10.Checked))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("You must enter a player name and a game board size!");
            }
        }

        private void checkBoxSecondPlayer_CheckedChanged(object i_Sender, EventArgs i_EventArguments)
        {
            textBoxSecondPlayerName.Enabled = checkBoxSecondPlayer.Checked;
            textBoxSecondPlayerName.Text = checkBoxSecondPlayer.Checked ? string.Empty : "[Computer]";
        }

        private void radioButton6x6_CheckedChanged(object i_Sender, EventArgs i_EventArguments)
        {
            m_BoardSize = eCheckersBoardSize.SmallSize;
        }

        private void radioButton8x8_CheckedChanged(object i_Sender, EventArgs i_EventArguments)
        {
            m_BoardSize = eCheckersBoardSize.MediumSize;
        }

        private void radioButton10x10_CheckedChanged(object i_Sender, EventArgs i_EventArguments)
        {
            m_BoardSize = eCheckersBoardSize.LargeSize;
        }

        protected override void OnFormClosing(FormClosingEventArgs i_FormClosingArgs)
        {
            base.OnFormClosing(i_FormClosingArgs);
        }
    }
}
