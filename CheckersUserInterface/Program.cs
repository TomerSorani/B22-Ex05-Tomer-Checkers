using System.Windows.Forms;

namespace CheckersUserInterface
{
    public static class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();

            CheckersGameSettings checkersGameSettings = new CheckersGameSettings();
            
            checkersGameSettings.ShowDialog();
            if(checkersGameSettings.DialogResult == DialogResult.OK)
            {
                CheckersUi checkersUi = new CheckersUi(checkersGameSettings);

                checkersUi.ShowDialog();
            }
        }
    }
}
