using System;
using System.Collections.Generic;
using System.Linq;
using CheckersEngine.Enums;

namespace CheckersEngine
{
    public class CheckersGameEngine
    {
        private readonly eGameMode r_GameMode;
        private readonly CheckersPlayer r_FirstPlayer, r_SecondPlayer;
        private readonly CheckersBoard r_CheckersGameBoard;
        private CheckersBoardSlot m_LastPlayedBoardSlot;
        private CheckersPlayerTurn m_CurrentTurn;
        private ePlayerTurn m_PlayerTurn;
        private ePlayerTurn? m_LastPlayerPlayed;
        private bool m_MoveHappend;

        public event Action ScoreChanged;

        public CheckersGameEngine(string i_FirstPlayerName, string i_SecondPlayerName, eGameMode i_GameMode, eCheckersBoardSize i_BoardSize)
        {
            r_GameMode = i_GameMode;
            r_CheckersGameBoard = new CheckersBoard(i_BoardSize);
            r_FirstPlayer = new CheckersPlayer(i_FirstPlayerName);
            r_SecondPlayer = new CheckersPlayer(i_SecondPlayerName);
            m_PlayerTurn = ePlayerTurn.FirstPlayerTurn;
            m_LastPlayerPlayed = null;
            m_MoveHappend = false;
            m_LastPlayedBoardSlot = null;
            m_CurrentTurn = null;

            if (GameMode == eGameMode.PlayAgainstTheComputerMode)
            {
                CheckersBoardSlot startMoveBoardSlotForComputer = r_CheckersGameBoard.GetSlotsFromSpecificRowThatHavePieces(out CheckersBoardSlot futureComputerMove);
                
                m_CurrentTurn = new CheckersPlayerTurn(
                    startMoveBoardSlotForComputer,
                    futureComputerMove,
                    ePlayerTurn.SecondPlayerTurn,
                    r_CheckersGameBoard);
            }
        }

        public CheckersPlayer FirstPlayer
        {
            get
            {
                return r_FirstPlayer;
            }
        }

        public CheckersPlayer SecondPlayer
        {
            get
            {
                return r_SecondPlayer;
            }
        }

        public bool MoveHappend
        {
            get
            {
                return m_MoveHappend;
            }

            set
            {
                m_MoveHappend = value;
            }
        }

        public eMatchResult UpdateMatchPointsAndGetResultOfTheMatch()
        {
            int currentMatchScore = r_CheckersGameBoard.GetCurrentMatchScore(out eMatchResult matchResult);

            switch(matchResult)
            {
                case eMatchResult.FirstPlayerWon:
                    {
                        r_FirstPlayer.UpdatePlayerScore(currentMatchScore);
                        break;
                    }

                case eMatchResult.SecondPlayerWon:
                    {
                        r_SecondPlayer.UpdatePlayerScore(currentMatchScore);
                        break;
                    }

                case eMatchResult.Tie:
                    {
                        r_FirstPlayer.UpdatePlayerScore(currentMatchScore);
                        r_SecondPlayer.UpdatePlayerScore(currentMatchScore);
                        break;
                    }
            }

            TheScoreChanged();

            return matchResult;
        }

        public CheckersPlayerTurn CurrentTurn
        {
            get
            {
                return m_CurrentTurn;
            }

            set
            {
                m_CurrentTurn = value;
            }
        }

        public eGameMode GameMode
        {
            get
            {
                return r_GameMode;
            }
        }

        public CheckersBoardSlot LastPlayedBoardSlot
        {
            get
            {
                return m_LastPlayedBoardSlot;
            }

            set
            {
                m_LastPlayedBoardSlot = value;
            }
        }

        public ePlayerTurn PlayerTurn
        {
            get
            {
                return ((ushort)m_PlayerTurn % 2 == 0) ? ePlayerTurn.FirstPlayerTurn : ePlayerTurn.SecondPlayerTurn;
            }

            set
            {
                m_PlayerTurn = value;
            }
        }

        public ePlayerTurn? LastPlayerPlayed
        {
            get
            {
                return m_LastPlayerPlayed;
            }

            set
            {
                m_LastPlayerPlayed = value;
            }
        }

        public CheckersBoard CheckersGameBoard
        {
            get
            {
                return r_CheckersGameBoard;
            }
        }

        public void StartOverTheMatch()
        {
            m_PlayerTurn = ePlayerTurn.FirstPlayerTurn;
            m_MoveHappend = false;
            m_LastPlayedBoardSlot = null;
            m_LastPlayerPlayed = null;
            resetCurrentGameScoreAndAddToTotalScore();
            initBoard();
        }

        private void initBoard()
        {
            short boardRowSize = (short) r_CheckersGameBoard.BoardRowSize;

            r_CheckersGameBoard.InitBoard(boardRowSize);
        }

        private void resetCurrentGameScoreAndAddToTotalScore()
        {
           r_FirstPlayer.PrepareTheScoreToNewMatch();
           r_SecondPlayer.PrepareTheScoreToNewMatch();
           r_CheckersGameBoard.DepletePlayerSlotListAndRemovePiecesFromBoard(ePlayerTurn.FirstPlayerTurn);
           r_CheckersGameBoard.DepletePlayerSlotListAndRemovePiecesFromBoard(ePlayerTurn.SecondPlayerTurn);
        }

        public void ChangePlayerTurn()
        {
            PlayerTurn = m_PlayerTurn == ePlayerTurn.FirstPlayerTurn
                             ? ePlayerTurn.SecondPlayerTurn
                             : ePlayerTurn.FirstPlayerTurn;
        }

        public bool CheckIfGameIsOver()
        { 
            bool atLeastOneOfThePlayersDoesNotHaveAnyPiecesLeft = r_CheckersGameBoard.FirstPlayerSlotList.Count == 0 ||
                                                                  r_CheckersGameBoard.SecondPlayerSlotList.Count == 0;

            return atLeastOneOfThePlayersDoesNotHaveAnyPiecesLeft || !checkIfPlayerHasAnyValidMovesToDo(ePlayerTurn.FirstPlayerTurn) || 
                   !checkIfPlayerHasAnyValidMovesToDo(ePlayerTurn.SecondPlayerTurn);
        }

        private bool checkIfPlayerHasAnyValidMovesToDo(ePlayerTurn i_Player)
        {
            bool foundAvailableMove;
            const bool v_IsFirstPlayer = true;

            if (i_Player == ePlayerTurn.FirstPlayerTurn)
            {
                foundAvailableMove = checkIfThereIsAvailableMove(r_CheckersGameBoard.FirstPlayerSlotList, v_IsFirstPlayer);
            }
            else
            {
                foundAvailableMove = checkIfThereIsAvailableMove(r_CheckersGameBoard.SecondPlayerSlotList, !v_IsFirstPlayer);
            }

            return foundAvailableMove;
        }

        private bool checkIfThereIsAvailableMove(LinkedList<CheckersBoardSlot> i_CurrentBoardSlotsList, bool i_IsFirstPlayer)
        {
            bool foundAvailableMove = false;

            foreach(CheckersBoardSlot currentBoardSlot in i_CurrentBoardSlotsList)
            {
                bool isThereNormalMove = CurrentTurn.CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(
                                                 currentBoardSlot,
                                                 i_IsFirstPlayer,
                                                 eDirectionMove.NormalUpOrDown,
                                                 null,
                                                 null);
                bool isThereEatingMove = CurrentTurn.CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(
                    currentBoardSlot,
                    i_IsFirstPlayer,
                    eDirectionMove.DoubleUpOrDoubleDown,
                    null,
                    null);

                if (isThereNormalMove || isThereEatingMove)
                {
                    foundAvailableMove = true;
                    break;
                }
            }

            return foundAvailableMove;
        }

        public void PlayerLeftMatchHandle()
        {
            r_CheckersGameBoard.DepletePlayerSlotListAndRemovePiecesFromBoard(m_PlayerTurn);
        }

        public CheckersPlayerTurn GetValidComputerMove()
        {
            r_CheckersGameBoard.CalculateComputerMoves(m_CurrentTurn, m_LastPlayedBoardSlot);

            int amountOfValidMovesForComputerToDo = r_CheckersGameBoard.ComputerValidMovesList.Count;
            Random random = new Random();
            int randomMoveIndex = random.Next(0, amountOfValidMovesForComputerToDo - 1);

            return r_CheckersGameBoard.ComputerValidMovesList.ElementAt(randomMoveIndex);
        }

        public void TheScoreChanged()
        {
            OnScoreChanged();
        }

        protected virtual void OnScoreChanged()
        {
            if (ScoreChanged != null)
            {
                ScoreChanged.Invoke();
            }
        }
    }
}

