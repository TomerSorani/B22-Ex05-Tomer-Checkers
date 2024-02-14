using System.Drawing;
using System.Windows.Forms;
using CheckersEngine.Enums;
using CheckersEngine;

namespace CheckersUserInterface
{
    public class UiBoardSlot : Button
    {
        private const int k_Size = 50;
        private readonly int r_Row;
        private readonly int r_Col;
        private bool m_Pressed;

        public UiBoardSlot(int i_Row, int i_Col)
        {
            m_Pressed = false;
            r_Row = i_Row;
            r_Col = i_Col;

            Size = new Size(k_Size, k_Size);
            
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        public bool Pressed
        {
            get
            {
                return m_Pressed;
            }

            set
            {
                m_Pressed = value;
            }
        }

        public int BoardSlotSize
        {
            get
            {
                return k_Size;
            }
        }

        public BoardPosition BoardPosition
        {
            get
            {
                return new BoardPosition((short)r_Row, (short)r_Col);
            }
        }

        public void UpdatePictureAccordingToPieceTypeAndOwnership(ePieceTypeAndOwnershipInfoInSlot i_PieceTypeAndOwnershipInfo)
        {
            switch (i_PieceTypeAndOwnershipInfo)
            {
                case ePieceTypeAndOwnershipInfoInSlot.FirstPlayerPiece:
                    BackgroundImage = Pressed ? Properties.Resources.pressedX : Properties.Resources.X;
                    break;
                case ePieceTypeAndOwnershipInfoInSlot.SecondPlayerPiece:
                    BackgroundImage = Pressed ? Properties.Resources.pressedO : Properties.Resources.O;
                    break;
                case ePieceTypeAndOwnershipInfoInSlot.FirstPlayerKingPiece:
                    BackgroundImage = Pressed ? Properties.Resources.pressedXking : Properties.Resources.Xking;
                    break;
                case ePieceTypeAndOwnershipInfoInSlot.SecondPlayerKingPiece:
                    BackgroundImage = Pressed ? Properties.Resources.pressedOking : Properties.Resources.Oking;
                    break;
                case ePieceTypeAndOwnershipInfoInSlot.None:
                    BackgroundImage = null;
                    break;
            }
        }
    }
}
