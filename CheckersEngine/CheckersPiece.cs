using CheckersEngine.Enums;

namespace CheckersEngine
{
    public class CheckersPiece
    {
        private ePieceType m_PieceType;
        private ePlayerPieceOwner m_PlayerPieceOwner;

        public CheckersPiece(ePlayerPieceOwner i_PlayerPieceOwner, ePieceType i_PieceType)
        {
            m_PlayerPieceOwner = i_PlayerPieceOwner;
            m_PieceType = i_PieceType;
        }

        public ePieceType PieceType
        {
            get
            {
                return m_PieceType;
            }

            set
            {
                m_PieceType = value;
            }
        }
        
        public ePlayerPieceOwner PlayerPieceOwner
        {
            get
            {
                return m_PlayerPieceOwner;
            }

            set
            {
                m_PlayerPieceOwner = value;
            }
        }
    }
}