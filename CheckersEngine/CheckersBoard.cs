using System;
using System.Collections.Generic;
using System.Linq;
using CheckersEngine.Enums;

namespace CheckersEngine
{
    public class CheckersBoard
    {
        private readonly CheckersBoardSlot[,] r_BoardMatrix;
        private readonly LinkedList<CheckersBoardSlot> r_FirstPlayerSlotsList, r_SecondPlayerSlotsList;
        private readonly LinkedList<CheckersPlayerTurn> r_ComputerValidMovesList;

        public CheckersBoard(eCheckersBoardSize i_Size)
        {
            short checkersBoardSizeAsUshort = (short)i_Size;
            r_BoardMatrix = new CheckersBoardSlot[checkersBoardSizeAsUshort, checkersBoardSizeAsUshort];
            r_FirstPlayerSlotsList = new LinkedList<CheckersBoardSlot>();
            r_SecondPlayerSlotsList = new LinkedList<CheckersBoardSlot>();
            r_ComputerValidMovesList = new LinkedList<CheckersPlayerTurn>();

            InitBoard(checkersBoardSizeAsUshort);
        }

        public void InitBoard(short i_CheckersBoardSizeAsUshort)
        {
            for (short row = 0; row < i_CheckersBoardSizeAsUshort; row++)
            {
                for (short col = 0; col < i_CheckersBoardSizeAsUshort; col++)
                {
                    calculateBoardSlotAndAllocateInBoard(
                        row,
                        col,
                        i_CheckersBoardSizeAsUshort,
                        out r_BoardMatrix[row, col]);
                }
            }
        }

        public int GetCurrentMatchScore(out eMatchResult io_MatchResult)
        {
            int firstPlayerPointsOnBoard = getPointsAccordingToAmountOfPiecesOnBoard(FirstPlayerSlotList);
            int secondPlayerPointsOnBoard = getPointsAccordingToAmountOfPiecesOnBoard(SecondPlayerSlotList);
            io_MatchResult = eMatchResult.FirstPlayerWon;

            if (firstPlayerPointsOnBoard == secondPlayerPointsOnBoard)
            {
                io_MatchResult = eMatchResult.Tie;
            }
            else if (secondPlayerPointsOnBoard > firstPlayerPointsOnBoard)
            {
                io_MatchResult = eMatchResult.SecondPlayerWon;
            }

            return Math.Abs(firstPlayerPointsOnBoard - secondPlayerPointsOnBoard);
        }

        private int getPointsAccordingToAmountOfPiecesOnBoard(LinkedList<CheckersBoardSlot> i_PlayerBoardSlotsListToCheck)
        {
            int playerPointsOnBoard = 0;

            foreach (CheckersBoardSlot currentBoardSlot in i_PlayerBoardSlotsListToCheck)
            {
                playerPointsOnBoard += (int)currentBoardSlot.Piece.PieceType;
            }

            return playerPointsOnBoard;
        }

        private void calculateBoardSlotAndAllocateInBoard(
                short i_Row,
                short i_Col,
                short i_CheckersBoardSizeAsUshort,
                out CheckersBoardSlot o_NewCheckersBoardSlot)
        {
            ePlayerPieceOwner currentPlayer = ePlayerPieceOwner.FirstPlayerPiece;
            BoardPosition currentBoardPosition = new BoardPosition(i_Row, i_Col);
            bool isAllowedToPutPieceOnThisPositionOnBoard = i_Row % 2 != i_Col % 2;
            bool isShouldBeEmptySlot = (i_Row == ((i_CheckersBoardSizeAsUshort / 2) - 1)) || (i_Row == ((i_CheckersBoardSizeAsUshort / 2)));

            if (!isAllowedToPutPieceOnThisPositionOnBoard)
            {
                o_NewCheckersBoardSlot = new CheckersBoardSlot(
                    currentBoardPosition,
                    ePlayerPieceOwner.None,
                    ePieceType.None,
                    isAllowedToPutPieceOnThisPositionOnBoard);
            }
            else if (isShouldBeEmptySlot)
            {
                o_NewCheckersBoardSlot = new CheckersBoardSlot(
                    currentBoardPosition,
                    ePlayerPieceOwner.None,
                    ePieceType.None,
                    isShouldBeEmptySlot);
            }
            else
            {
                if (i_Row >= 0 && i_Row <= ((i_CheckersBoardSizeAsUshort / 2) - 1))
                {
                    currentPlayer = ePlayerPieceOwner.SecondPlayerPiece;
                }

                o_NewCheckersBoardSlot = new CheckersBoardSlot(
                    currentBoardPosition,
                    currentPlayer,
                    ePieceType.Regular,
                    isAllowedToPutPieceOnThisPositionOnBoard);
            }

            UpdateCheckersBoardSlotInUsersLists(o_NewCheckersBoardSlot);
        }

        public void UpdateCheckersBoardSlotInUsersLists(CheckersBoardSlot i_BoardSlotToHandle)
        {
            if (i_BoardSlotToHandle.Piece.PlayerPieceOwner == ePlayerPieceOwner.FirstPlayerPiece)
            {
                handleUpdateCheckersBoardSlotInUsersListsAddingCase(r_FirstPlayerSlotsList, r_SecondPlayerSlotsList, i_BoardSlotToHandle);
            }
            else if (i_BoardSlotToHandle.Piece.PlayerPieceOwner == ePlayerPieceOwner.SecondPlayerPiece)
            {
                handleUpdateCheckersBoardSlotInUsersListsAddingCase(r_SecondPlayerSlotsList, r_FirstPlayerSlotsList, i_BoardSlotToHandle);
            }
            else if (i_BoardSlotToHandle.Piece.PlayerPieceOwner == ePlayerPieceOwner.None)
            {
                if (r_SecondPlayerSlotsList.Contains(i_BoardSlotToHandle))
                {
                    r_SecondPlayerSlotsList.Remove(i_BoardSlotToHandle);
                }
                else if (r_FirstPlayerSlotsList.Contains(i_BoardSlotToHandle))
                {
                    r_FirstPlayerSlotsList.Remove(i_BoardSlotToHandle);
                }
            }
        }

        private void handleUpdateCheckersBoardSlotInUsersListsAddingCase(LinkedList<CheckersBoardSlot> i_FirstList, LinkedList<CheckersBoardSlot> i_SecondList, CheckersBoardSlot i_BoardSlotToHandle)
        {
            i_FirstList.AddLast(i_BoardSlotToHandle);
            if (i_SecondList.Contains(i_BoardSlotToHandle))
            {
                i_SecondList.Remove(i_BoardSlotToHandle);
            }
        }

        public CheckersBoardSlot GetSlotsFromSpecificRowThatHavePieces(out CheckersBoardSlot o_FutureComputerMove)
        {
            Random random = new Random();
            int randomBeginningMoveForComputerIndex = random.Next(0, (BoardRowSize / 2) - 1);
            int rowToStartFirstMoveToComputer = (BoardRowSize / 2) - 2;
            LinkedList<CheckersBoardSlot> listWithSpecificRowThatHavePieces = new LinkedList<CheckersBoardSlot>();

            for (int j = 0; j < BoardRowSize; j++)
            {
                if (r_BoardMatrix[rowToStartFirstMoveToComputer, j].Piece.PieceType != ePieceType.None)
                {
                    if ((j + (int)eDirectionMove.Left) >= 0 || (j + (int)eDirectionMove.Right) < BoardRowSize)
                    {
                        listWithSpecificRowThatHavePieces.AddLast(r_BoardMatrix[rowToStartFirstMoveToComputer, j]);
                    }
                }
            }

            CheckersBoardSlot randomStartSlot = listWithSpecificRowThatHavePieces.ElementAt(randomBeginningMoveForComputerIndex);

            if (randomStartSlot.Position.Col + (int)eDirectionMove.Left >= 0)
            {
                o_FutureComputerMove = r_BoardMatrix[randomStartSlot.Position.Row + (int)eDirectionMove.Down,
                    randomStartSlot.Position.Col + (int)eDirectionMove.Left];
            }
            else
            {
                o_FutureComputerMove = r_BoardMatrix[randomStartSlot.Position.Row + (int)eDirectionMove.Down,
                    randomStartSlot.Position.Col + (int)eDirectionMove.Right];
            }

            return randomStartSlot;
        }

        public int BoardRowSize
        {
            get
            {
                return (int)Math.Sqrt(r_BoardMatrix.Length);
            }
        }

        public LinkedList<CheckersBoardSlot> FirstPlayerSlotList
        {
            get
            {
                return r_FirstPlayerSlotsList;
            }
        }

        public LinkedList<CheckersBoardSlot> SecondPlayerSlotList
        {
            get
            {
                return r_SecondPlayerSlotsList;
            }
        }

        public LinkedList<CheckersPlayerTurn> ComputerValidMovesList
        {
            get
            {
                return r_ComputerValidMovesList;
            }
        }

        public ePieceTypeAndOwnershipInfoInSlot GetPieceOwnershipAndPieceTypeAccordingToBoardPosition(BoardPosition i_Position)
        {
            ePieceTypeAndOwnershipInfoInSlot pieceTypeAndOwnershipInfoInSlot = ePieceTypeAndOwnershipInfoInSlot.None;
            ePlayerPieceOwner playerPieceOwner = r_BoardMatrix[i_Position.Row, i_Position.Col].Piece.PlayerPieceOwner;
            ePieceType type = r_BoardMatrix[i_Position.Row, i_Position.Col].Piece.PieceType;

            switch(type)
            {
                case ePieceType.Regular:
                    {
                        pieceTypeAndOwnershipInfoInSlot = playerPieceOwner == ePlayerPieceOwner.SecondPlayerPiece
                                                              ? ePieceTypeAndOwnershipInfoInSlot.SecondPlayerPiece
                                                              : ePieceTypeAndOwnershipInfoInSlot.FirstPlayerPiece;
                        break;
                    }

                case ePieceType.King:
                    {
                        pieceTypeAndOwnershipInfoInSlot = playerPieceOwner == ePlayerPieceOwner.SecondPlayerPiece
                                                              ? ePieceTypeAndOwnershipInfoInSlot.SecondPlayerKingPiece
                                                              : ePieceTypeAndOwnershipInfoInSlot.FirstPlayerKingPiece;
                        break;
                    }
            }

            return pieceTypeAndOwnershipInfoInSlot;
        }

        public void DepletePlayerSlotListAndRemovePiecesFromBoard(ePlayerTurn i_PlayerTurn)
        {
            LinkedList<CheckersBoardSlot> checkersBoardSlotListToHandle = FirstPlayerSlotList;

            if (i_PlayerTurn == ePlayerTurn.SecondPlayerTurn)
            {
                checkersBoardSlotListToHandle = SecondPlayerSlotList;
            }

            foreach (CheckersBoardSlot currentCheckersBoardSlot in checkersBoardSlotListToHandle)
            {
                currentCheckersBoardSlot.Piece.PieceType = ePieceType.None;
                currentCheckersBoardSlot.Piece.PlayerPieceOwner = ePlayerPieceOwner.None;
            }

            checkersBoardSlotListToHandle.Clear();
        }

        public CheckersBoardSlot GetCheckersBoardSlot(short i_RowNumber, short i_ColNumber)
        {
            CheckersBoardSlot checkersBoardSlot = null;

            if (!(i_RowNumber >= BoardRowSize || i_ColNumber >= BoardRowSize || i_RowNumber < 0 || i_ColNumber < 0))
            {
                checkersBoardSlot = r_BoardMatrix[i_RowNumber, i_ColNumber];
            }

            return checkersBoardSlot;
        }

        public void CalculateComputerMoves(CheckersPlayerTurn i_CurrentTurn, CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            r_ComputerValidMovesList.Clear();
            foreach (CheckersBoardSlot currentBoardSlot in SecondPlayerSlotList)
            {
                const bool v_IsFirstPlayer = false;

                i_CurrentTurn.CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(
                    currentBoardSlot,
                    v_IsFirstPlayer,
                    eDirectionMove.DoubleUpOrDoubleDown,
                    r_ComputerValidMovesList,
                    i_LastPlayedBoardSlot);

                i_CurrentTurn.CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(
                    currentBoardSlot,
                    v_IsFirstPlayer,
                    eDirectionMove.NormalUpOrDown,
                    r_ComputerValidMovesList,
                    i_LastPlayedBoardSlot);
            }
        }
    }
}
