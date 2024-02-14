namespace CheckersEngine
{
    public class CheckersPlayer
    {
        private readonly string r_PlayerName;
        private int m_CurrentGameScore, m_TotalGamesScore;

        public CheckersPlayer(string i_PlayerName)
        {
            r_PlayerName = i_PlayerName;
            m_CurrentGameScore = 0;
            m_TotalGamesScore = 0;
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int TotalGamesScore
        {
            get
            {
                return m_TotalGamesScore;
            }

            set
            {
                m_TotalGamesScore = value;
            }
        }

        public void PrepareTheScoreToNewMatch()
        {
            m_CurrentGameScore = 0;
        }

        public void UpdatePlayerScore(int i_CurrentGameScore)
        {
             m_CurrentGameScore = i_CurrentGameScore;
             m_TotalGamesScore += m_CurrentGameScore;
        }
    }
}
