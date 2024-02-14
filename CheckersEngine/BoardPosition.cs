namespace CheckersEngine
{
    public class BoardPosition
    {
        private short m_Row, m_Col;

        public BoardPosition(short i_Row, short i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public short Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public short Col
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }
    }
}
