using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CheckersEngine.Enums;
using CheckersEngine;

namespace CheckersUserInterface
{
    public partial class CheckersUi : Form
    {
        private const int k_InitRowPosition = 150;
        private const int k_InitColPosition = 40;
        private const int k_MillSecondsWaitingTimer = 2000;
        private readonly Color r_InvalidColor;
        private readonly Color r_ValidColor;
        private readonly Timer r_ComputerTurnTimer;
        private bool m_AskNewGameOnClose;
        private UiBoardSlot[,] m_UiBoardMatrix;
        private CheckersGameEngine m_CheckersGameEngine;
        private UiBoardSlot m_SourceSlot, m_DestinationSlot;

        public CheckersUi(CheckersGameSettings i_CheckersGameSettings)
        {
            m_CheckersGameEngine = null;
            m_SourceSlot = null;
            m_DestinationSlot = null;
            m_AskNewGameOnClose = true;
            r_ComputerTurnTimer = new Timer();
            r_InvalidColor = Color.Tan;
            r_ValidColor = Color.WhiteSmoke;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            initWaitingTimer();
            InitializeComponent();
            createCheckersGameEngine(i_CheckersGameSettings);
            makeWindowSizeAccordingToOrder(i_CheckersGameSettings);
            m_CheckersGameEngine.ScoreChanged += m_CheckersGameEngine_ScoreChanged;
            m_CheckersGameEngine.UpdateMatchPointsAndGetResultOfTheMatch();
        }

        private void m_CheckersGameEngine_ScoreChanged()
        {
            labelPlayer1Points.Text = m_CheckersGameEngine.FirstPlayer.PlayerName + ": " + m_CheckersGameEngine.FirstPlayer.TotalGamesScore;
            labelPlayer2Points.Text = m_CheckersGameEngine.SecondPlayer.PlayerName + ": " + m_CheckersGameEngine.SecondPlayer.TotalGamesScore;
        }

        private void makeWindowSizeAccordingToOrder(CheckersGameSettings i_CheckersGameSettings)
        {
            const int k_MultInThis = 2;
            int doubleBoardSlotSize = k_MultInThis * m_UiBoardMatrix[0, 0].BoardSlotSize;

            switch (i_CheckersGameSettings.BoardSize)
            {
                case eCheckersBoardSize.SmallSize:
                    {
                        Width -= doubleBoardSlotSize;
                        Height -= doubleBoardSlotSize;
                        break;
                    }

                case eCheckersBoardSize.LargeSize:
                    {
                        Width += doubleBoardSlotSize;
                        Height += doubleBoardSlotSize;
                        break;
                    }
            }
        }

        private void initWaitingTimer()
        {
            r_ComputerTurnTimer.Interval = k_MillSecondsWaitingTimer;
            r_ComputerTurnTimer.Tick += stopTimerTickingAndUpdateBoardAfter;
        }

        private void updateLabelTurn()
        {
            labelTurn.Text = m_CheckersGameEngine.PlayerTurn == ePlayerTurn.FirstPlayerTurn ? 
                                 m_CheckersGameEngine.FirstPlayer.PlayerName
                                 : m_CheckersGameEngine.SecondPlayer.PlayerName;
        }

        private void stopTimerTickingAndUpdateBoardAfter(object i_Sender, EventArgs i_EventArguments)
        {
            r_ComputerTurnTimer.Stop();
            updateCheckersGameBoard();
        }

        private void createCheckersGameEngine(CheckersGameSettings i_CheckersGameSettings)
        {
            string firstPlayerName = i_CheckersGameSettings.FirstPlayerName;
            string secondPlayerName = i_CheckersGameSettings.SecondPlayerName;
            eGameMode gameMode = i_CheckersGameSettings.GameMode;
            eCheckersBoardSize checkersBoardSize = i_CheckersGameSettings.BoardSize;
            m_CheckersGameEngine = new CheckersGameEngine(firstPlayerName, secondPlayerName, gameMode, checkersBoardSize);

            initCheckersGameBoard((int)checkersBoardSize);
        }

        private void initCheckersGameBoard(int i_BoardSize)
        {
            m_UiBoardMatrix = new UiBoardSlot[i_BoardSize, i_BoardSize];

            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    m_UiBoardMatrix[row, col] = new UiBoardSlot(row, col);

                    Controls.Add(m_UiBoardMatrix[row, col]);
                    setPositionOfUiBoardSlotInUiMatrix(row, col);
                    setColorOfUiBoardSlotInUiMatrix(row, col);
                    updatePieceOnUiBoard(row, col);
                    m_UiBoardMatrix[row, col].Click += m_UiBoardMatrix_Click;
                }
            }

            updateLabelTurn();
        }

        private void setPositionOfUiBoardSlotInUiMatrix(int i_Row, int i_Col)
        {
            const int k_FirstCol = 0;

            if (i_Col == k_FirstCol)
            {
                m_UiBoardMatrix[i_Row, i_Col].Top = i_Row == k_FirstCol ? k_InitColPosition : m_UiBoardMatrix[i_Row - 1, i_Col].Bottom;
                m_UiBoardMatrix[i_Row, i_Col].Left = k_InitRowPosition;
            }
            else
            {
                m_UiBoardMatrix[i_Row, i_Col].Top = m_UiBoardMatrix[i_Row, i_Col - 1].Top;
                m_UiBoardMatrix[i_Row, i_Col].Left = m_UiBoardMatrix[i_Row, i_Col - 1].Right;
            }
        }

        private void setColorOfUiBoardSlotInUiMatrix(int i_Row, int i_Col)
        {
            if (m_CheckersGameEngine.CheckersGameBoard.GetCheckersBoardSlot((short)i_Row, (short)i_Col)
               .IsAllowedToPutPieceOnThisPositionOnBoard)
            {
                m_UiBoardMatrix[i_Row, i_Col].BackColor = r_ValidColor;
            }
            else
            {
                m_UiBoardMatrix[i_Row, i_Col].BackColor = r_InvalidColor;
                m_UiBoardMatrix[i_Row, i_Col].Enabled = false;
            }
        }

        private void updatePieceOnUiBoard(int i_Row, int i_Col)
        {
            ePieceTypeAndOwnershipInfoInSlot pieceTypeAndOwnershipInfo =
                m_CheckersGameEngine.CheckersGameBoard.GetPieceOwnershipAndPieceTypeAccordingToBoardPosition(
                    new BoardPosition((short)i_Row, (short)i_Col));

            m_UiBoardMatrix[i_Row, i_Col].UpdatePictureAccordingToPieceTypeAndOwnership(pieceTypeAndOwnershipInfo);
        }

        private void updateCheckersGameBoard()
        {
            for (int row = 0; row < m_CheckersGameEngine.CheckersGameBoard.BoardRowSize; row++)
            {
                for (int col = 0; col < m_CheckersGameEngine.CheckersGameBoard.BoardRowSize; col++)
                {
                    updatePieceOnUiBoard(row, col);
                }
            }

            updateLabelTurn();
        }

        private void resetSourceAndDestinationUiBoardSlotsAndUpdateSourceSlotToNonePressedIfNecessary()
        {
            if (m_SourceSlot != null)
            {
                m_SourceSlot.Pressed = false;
                updatePieceOnUiBoard(m_SourceSlot.BoardPosition.Row, m_SourceSlot.BoardPosition.Col);
            }

            m_SourceSlot = null;
            m_DestinationSlot = null;
            labelPlayer1Points.Focus();
        }   

        private void updateCurrentTurn()
        {
            if (checkIfComputerIsPlayingNow())
            {
                m_CheckersGameEngine.CurrentTurn = m_CheckersGameEngine.GetValidComputerMove();
            }
            else
            {
                CheckersBoardSlot currentBoardSlot = m_CheckersGameEngine.CheckersGameBoard.GetCheckersBoardSlot(
                    m_SourceSlot.BoardPosition.Row,
                    m_SourceSlot.BoardPosition.Col);

                CheckersBoardSlot futureBoardSlot = m_CheckersGameEngine.CheckersGameBoard.GetCheckersBoardSlot(
                    m_DestinationSlot.BoardPosition.Row,
                    m_DestinationSlot.BoardPosition.Col);

                m_CheckersGameEngine.CurrentTurn = new CheckersPlayerTurn(
                    currentBoardSlot,
                    futureBoardSlot,
                   m_CheckersGameEngine.PlayerTurn,
                   m_CheckersGameEngine.CheckersGameBoard);
            }
        }

        private void createTurnAndDoIt()
        {
            bool changeTurn, pieceBecameKing, isGameFinished = false;
            CheckersPiece eatenPiece;

            updateCurrentTurn();

            eMoveStatus moveStatus = m_CheckersGameEngine.CurrentTurn.CheckIfTurnIsValid(m_CheckersGameEngine.LastPlayedBoardSlot);

            if (!checkMoveStatusAndDetailsAndCheckIfErrorExists(moveStatus))
            {
                m_CheckersGameEngine.MoveHappend = true;
                m_CheckersGameEngine.LastPlayedBoardSlot = m_CheckersGameEngine.CurrentTurn.MakeMove(out changeTurn, out eatenPiece, out pieceBecameKing);
                m_CheckersGameEngine.LastPlayerPlayed = m_CheckersGameEngine.PlayerTurn;
                turnOnComputerTimerIfItsComputerTurn();
                resetSourceAndDestinationUiBoardSlotsAndUpdateSourceSlotToNonePressedIfNecessary();
                if (!changeTurn)
                {
                    updateCheckersGameBoard();
                    checkMoveStatusAndDetailsAndCheckIfErrorExists(eMoveStatus.HaveAnotherEatingMoveWithTheLastPiecePlayed);
                }
                else
                {
                    isGameFinished = m_CheckersGameEngine.CheckIfGameIsOver();
                    m_CheckersGameEngine.ChangePlayerTurn();
                }
            }

            resetSourceAndDestinationUiBoardSlotsAndUpdateSourceSlotToNonePressedIfNecessary();
            if (isGameFinished)
            {
                const bool v_GameFinishedByUser = false;

                if (!checkIfPlayAgainAndShowFinalResult(v_GameFinishedByUser))
                {
                    m_AskNewGameOnClose = false;
                    Close();
                }
            }

            if (!(m_CheckersGameEngine.PlayerTurn == ePlayerTurn.FirstPlayerTurn &&
                  m_CheckersGameEngine.GameMode == eGameMode.PlayAgainstTheComputerMode) && !isGameFinished)
            {
                updateCheckersGameBoard();
            }
        }

        private void turnOnComputerTimerIfItsComputerTurn()
        {
            if (m_CheckersGameEngine.PlayerTurn == ePlayerTurn.FirstPlayerTurn && m_CheckersGameEngine.GameMode == eGameMode.PlayAgainstTheComputerMode)
            {
                r_ComputerTurnTimer.Start();
            }
        }

        private void m_UiBoardMatrix_Click(object i_Sender, EventArgs i_EventArguments)
        {
            UiBoardSlot clickedSlot = (UiBoardSlot)i_Sender;

            if (!clickedSlot.Pressed)
            {
                if (m_SourceSlot == null)
                {
                    clickedSlot.Pressed = true;
                    m_SourceSlot = clickedSlot;
                    updatePieceOnUiBoard(clickedSlot.BoardPosition.Row, clickedSlot.BoardPosition.Col);
                }
                else
                {
                    m_DestinationSlot = clickedSlot;
                    do
                    {
                        updateCheckersGameBoard();
                        createTurnAndDoIt();
                    }
                    while (checkIfComputerIsPlayingNow());
                }
            }
            else 
            {
                clickedSlot.Pressed = false;
                updatePieceOnUiBoard(clickedSlot.BoardPosition.Row, clickedSlot.BoardPosition.Col);
                m_SourceSlot = null;
            }
        }

        private bool checkIfComputerIsPlayingNow()
        {
            return m_CheckersGameEngine.GameMode == eGameMode.PlayAgainstTheComputerMode
                   && m_CheckersGameEngine.PlayerTurn == ePlayerTurn.SecondPlayerTurn;
        }

        private bool checkMoveStatusAndDetailsAndCheckIfErrorExists(eMoveStatus i_ValidTurnException)
        {
            bool isThereException = true;

            if (checkIfComputerIsPlayingNow() && i_ValidTurnException == eMoveStatus.Valid)
            {
                isThereException = false;
            }
            else
            {
                string errorString = string.Empty;

                switch (i_ValidTurnException)
                {
                    case eMoveStatus.Valid:
                        {
                            isThereException = false;
                            break;
                        }

                    case eMoveStatus.ErrorOccurredDuringMove:
                        {
                            errorString = "Error occurred during move";
                            break;
                        }

                    case eMoveStatus.NotAllowedSlot:
                        {
                            errorString = "Invalid move: not allowed slot, please try again: ";
                            break;
                        }

                    case eMoveStatus.FutureBoardSlotIsNotAvailable:
                        {
                            errorString = "Invalid move: the slot you want to go to is not available, please try again: ";
                            break;
                        }

                    case eMoveStatus.MustDoEatingMove:
                        {
                            errorString = "Invalid move: you have have available eating move you must do it!, please try again: ";
                            break;
                        }

                    case eMoveStatus.HaveAnotherEatingMoveWithTheLastPiecePlayed:
                        {
                            errorString = "You have have one more available eating move with the last piece played!, you must do it!";
                            break;
                        }

                    case eMoveStatus.DoNotHavePlayerPieceInSlot:
                        {
                            errorString = "Invalid move: you don't have a game piece in the selected slot, please try again: ";
                            break;
                        }

                    case eMoveStatus.SlotsAreNotAdjacent:
                        {
                            errorString = "Invalid move: the slots are not adjacent, please try again: ";
                            break;
                        }

                    case eMoveStatus.InvalidMoveAccordingToPieceType:
                        {
                            errorString = "Invalid move: Invalid move for the selected piece type, please try again: ";
                            break;
                        }
                }

                if (isThereException && !checkIfComputerIsPlayingNow())
                {
                    MessageBox.Show(errorString);
                }
            }

            return isThereException;
        }

        private bool checkIfPlayAgainAndShowFinalResult(bool i_GameFinishedByUser)
        {
            bool playAgain = false;

            if (i_GameFinishedByUser)
            {
                m_CheckersGameEngine.PlayerLeftMatchHandle();
            }
            else
            {
                updateCheckersGameBoard();
            }

            showFinalResult();
            resetMatch();
            updateCheckersGameBoard();
            if (MessageBox.Show("Do you want to play another round?", "Play another round", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                playAgain = true;
            }

            return playAgain;
        }

        private void resetMatch()
        {
            m_CheckersGameEngine.StartOverTheMatch();
            resetSourceAndDestinationUiBoardSlotsAndUpdateSourceSlotToNonePressedIfNecessary();
        }

        private void showFinalResult()
        {
            eMatchResult winnerResult = m_CheckersGameEngine.UpdateMatchPointsAndGetResultOfTheMatch();
            string firstPlayerName = m_CheckersGameEngine.FirstPlayer.PlayerName;
            string secondPlayerName = m_CheckersGameEngine.SecondPlayer.PlayerName;
            string winnerName = firstPlayerName, message = string.Empty;

            if (winnerResult == eMatchResult.SecondPlayerWon)
            {
                winnerName = secondPlayerName;
            }
            else if (winnerResult == eMatchResult.Tie)
            {
                message = "The match has finished with tie!" + Environment.NewLine;
            }

            if (winnerResult != eMatchResult.Tie)
            {
                message = string.Format(
                    @"{0} won!!
", winnerName);
            }

            showTotalScore(firstPlayerName, secondPlayerName, message);
        }

        private void showTotalScore(string i_FirstPlayerName, string i_SecondPlayerName, string i_Title)
        {
            StringBuilder totalScoreBuilder = new StringBuilder(i_Title);

            totalScoreBuilder.AppendLine(string.Format(
                @"The total score till now:
{0} : {1}
{2} : {3}
",
                i_FirstPlayerName,
                m_CheckersGameEngine.FirstPlayer.TotalGamesScore,
                i_SecondPlayerName,
                m_CheckersGameEngine.SecondPlayer.TotalGamesScore));
            MessageBox.Show(totalScoreBuilder.ToString());
        }

        protected override void OnFormClosing(FormClosingEventArgs i_FormClosingArgs)
        {
            const bool v_GameFinishedByUser = true;

            base.OnFormClosing(i_FormClosingArgs);
            if (r_ComputerTurnTimer.Enabled || checkIfComputerIsPlayingNow())
            {
                MessageBox.Show("You can quit only on your turn ! don't be a loser!");
                i_FormClosingArgs.Cancel = true;
            }
            else if (m_AskNewGameOnClose)
            { 
                i_FormClosingArgs.Cancel = checkIfPlayAgainAndShowFinalResult(v_GameFinishedByUser);
            }
        }
    }
}
