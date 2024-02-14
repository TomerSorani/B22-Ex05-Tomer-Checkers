using System.Text;
using CheckersEngine.Enums;

namespace CheckersEngine
{
    public class CheckersBoardSlot
    {
        private readonly BoardPosition r_Position;
        private readonly bool r_IsAllowedToPutPieceOnThisPositionOnBoard;
        private CheckersPiece m_Piece;

        public CheckersBoardSlot(
            BoardPosition i_Position,
            ePlayerPieceOwner i_PlayerPieceOwner,
            ePieceType i_PieceType,
            bool i_IsAllowedToPutPieceOnThisPositionOnBoard)
        {
            r_Position = i_Position;
            m_Piece = new CheckersPiece(i_PlayerPieceOwner, i_PieceType);
            r_IsAllowedToPutPieceOnThisPositionOnBoard = i_IsAllowedToPutPieceOnThisPositionOnBoard;
        }

        public BoardPosition Position
        {
            get
            {
                return r_Position;
            }
        }

        public bool IsAllowedToPutPieceOnThisPositionOnBoard
        {
            get
            {
                return r_IsAllowedToPutPieceOnThisPositionOnBoard;
            }
        }

        public CheckersPiece Piece
        {
            get
            {
                return m_Piece;
            }

            set
            {
                m_Piece = value;
            }
        }

        public override string ToString()
        {
            StringBuilder resultStringBuilder = new StringBuilder();
            char col = (char)(r_Position.Col + 'A');
            char row = (char)(r_Position.Row + 'a');

            resultStringBuilder.Append(col.ToString() + row.ToString());

            return resultStringBuilder.ToString();
        }
    }
}
