using System.Collections.Generic;
using System.Text;
using CheckersEngine.Enums;

namespace CheckersEngine
{
    public class CheckersPlayerTurn
    {
        private readonly CheckersBoardSlot r_CurrentBoardSlot, r_FutureBoardSlot;
        private readonly bool r_IsFirstPlayerPlaying;
        private readonly ePlayerTurn r_CurrentPlayerPlaying;
        private readonly CheckersBoard r_GameBoard;
        private const bool v_UpdateEatingTargetSlotIfValid = true;
        private bool m_IsEatingMove;
        private BoardPosition m_EatingTargetBoardPosition = null;
        private eMoveStatus m_MoveStatus;

        public CheckersPlayerTurn( 
            CheckersBoardSlot i_CurrentBoardSlot,
            CheckersBoardSlot i_FutureBoardSlot,
            ePlayerTurn i_CurrentPlayerPlaying,
            CheckersBoard i_GameBoard)
        {
            r_CurrentBoardSlot = i_CurrentBoardSlot;
            r_FutureBoardSlot = i_FutureBoardSlot;
            r_CurrentPlayerPlaying = i_CurrentPlayerPlaying;
            m_MoveStatus = eMoveStatus.MoveHasNotCheckedYet;
            r_GameBoard = i_GameBoard;
            m_IsEatingMove = false;
            r_IsFirstPlayerPlaying = r_CurrentPlayerPlaying == ePlayerTurn.FirstPlayerTurn;
        }

        public CheckersBoardSlot MakeMove(out bool o_ChangeTurn, out CheckersPiece o_EatenPiece, out bool o_PieceBecameKing)
        {
            CheckersBoardSlot lastPlayedBoardSlot = null;
            o_ChangeTurn = m_MoveStatus == eMoveStatus.Valid;
            o_EatenPiece = new CheckersPiece(ePlayerPieceOwner.None, ePieceType.None);
            o_PieceBecameKing = false;

            if (o_ChangeTurn)
            {
                r_FutureBoardSlot.Piece.PlayerPieceOwner = r_CurrentBoardSlot.Piece.PlayerPieceOwner;
                r_FutureBoardSlot.Piece.PieceType = r_CurrentBoardSlot.Piece.PieceType;
                r_CurrentBoardSlot.Piece.PlayerPieceOwner = ePlayerPieceOwner.None;
                r_CurrentBoardSlot.Piece.PieceType = ePieceType.None;
                r_GameBoard.UpdateCheckersBoardSlotInUsersLists(r_CurrentBoardSlot);
                r_GameBoard.UpdateCheckersBoardSlotInUsersLists(r_FutureBoardSlot);
                if (m_IsEatingMove)
                {
                    CheckersBoardSlot eatingBoardSlot = r_GameBoard.GetCheckersBoardSlot(m_EatingTargetBoardPosition.Row, m_EatingTargetBoardPosition.Col);

                    eatingBoardSlot.Piece.PlayerPieceOwner = ePlayerPieceOwner.None;
                    eatingBoardSlot.Piece.PieceType = ePieceType.None;
                    r_GameBoard.UpdateCheckersBoardSlotInUsersLists(eatingBoardSlot);
                    o_EatenPiece = eatingBoardSlot.Piece;
                    if (CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(r_FutureBoardSlot, r_IsFirstPlayerPlaying, eDirectionMove.DoubleUpOrDoubleDown, null, null))
                    {
                        lastPlayedBoardSlot = r_FutureBoardSlot;
                        o_ChangeTurn = false;
                    }
                }

                updatePieceToKingIfNecessary(ref o_PieceBecameKing);
            }

            return lastPlayedBoardSlot;
        }

        private void updatePieceToKingIfNecessary(ref bool io_PieceBecameKing)
        {
            io_PieceBecameKing = false;
            bool isPieceIsRegular = r_FutureBoardSlot.Piece.PieceType == ePieceType.Regular;
            bool isFirstPlayerPlayingAndArrivedToTheTop = r_IsFirstPlayerPlaying && r_FutureBoardSlot.Position.Row == 0;
            bool isSecondPlayerPlayingAndArrivedToTheBottom = 
                !r_IsFirstPlayerPlaying && r_FutureBoardSlot.Position.Row == r_GameBoard.BoardRowSize - 1;

            if (isPieceIsRegular
               && (isFirstPlayerPlayingAndArrivedToTheTop || isSecondPlayerPlayingAndArrivedToTheBottom))
            {
                r_FutureBoardSlot.Piece.PieceType = ePieceType.King;
                io_PieceBecameKing = true;
            }
        }

        public eMoveStatus CheckIfTurnIsValid(CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            if(i_LastPlayedBoardSlot != null && r_CurrentBoardSlot != i_LastPlayedBoardSlot)
            {
                m_MoveStatus = eMoveStatus.HaveAnotherEatingMoveWithTheLastPiecePlayed;
            }
            else if (!r_FutureBoardSlot.IsAllowedToPutPieceOnThisPositionOnBoard)
            {
                m_MoveStatus = eMoveStatus.NotAllowedSlot;
            }
            else if (!checkIfThereIsPlayerPieceInCurrentBoardSlot())
            {
                m_MoveStatus = eMoveStatus.DoNotHavePlayerPieceInSlot;
            }
            else if (!checkIfFutureBoardSlotIsAvailable(r_FutureBoardSlot))
            {
                m_MoveStatus = eMoveStatus.FutureBoardSlotIsNotAvailable;
            }
            else if (checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(
                        r_CurrentBoardSlot,
                        r_FutureBoardSlot,
                        v_UpdateEatingTargetSlotIfValid))
            {
                m_IsEatingMove = true;
                m_MoveStatus = eMoveStatus.Valid;
            }
            else if (checkIfThereIsAnyAvailableEatingMovesToDo()) 
            {
                m_MoveStatus = eMoveStatus.MustDoEatingMove;
            }
            else if (!checkIfValidMoveAccordingToPieceType() && !m_IsEatingMove)
            {
                m_MoveStatus = eMoveStatus.InvalidMoveAccordingToPieceType;
            }
            else if (!checkAreSlotsAdjacent() && !m_IsEatingMove)
            {
                m_MoveStatus = eMoveStatus.SlotsAreNotAdjacent;
            }
            else
            {
                m_MoveStatus = eMoveStatus.Valid;
            }

            return m_MoveStatus;
        }

        private bool checkIfThereIsAnyAvailableEatingMovesToDo()
        {
            const bool v_IsFirstPlayerPlayingNow = true;

            return r_IsFirstPlayerPlaying ? 
                       checkIfThereAreAnyAvailableEatingMovesToDoForSpecificPlayer(v_IsFirstPlayerPlayingNow)
                       : checkIfThereAreAnyAvailableEatingMovesToDoForSpecificPlayer(!v_IsFirstPlayerPlayingNow);
        }

        private bool checkIfThereAreAnyAvailableEatingMovesToDoForSpecificPlayer(bool i_IsFirstPlayer)
        {
            bool isThereValidMove = false;
            LinkedList<CheckersBoardSlot> currentSlotListToCheck = r_GameBoard.FirstPlayerSlotList;

            if(!i_IsFirstPlayer)
            {
                currentSlotListToCheck = r_GameBoard.SecondPlayerSlotList;
            }

            foreach (CheckersBoardSlot currentSlot in currentSlotListToCheck)
            {
                if (isThereValidMove)
                {
                    break;
                }

                isThereValidMove = CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(currentSlot, i_IsFirstPlayer, eDirectionMove.DoubleUpOrDoubleDown, null, null);
            }

            return isThereValidMove;
        }

        public bool CheckIfThereAnyValidMoveFromSpecificBoardSlotAndPlayer(CheckersBoardSlot i_CurrentSlot, bool i_IsFirstPlayer, eDirectionMove i_AmountOfJump, LinkedList<CheckersPlayerTurn> i_ComputerValidTurnMoves, CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            short upRowOrDoubleUpRow = (short)(i_CurrentSlot.Position.Row + (short)i_AmountOfJump * (short)eDirectionMove.Up);
            short downRowOrDoubleDownRow = (short)(i_CurrentSlot.Position.Row + (short)i_AmountOfJump * (short)eDirectionMove.Down);
            short rightColOrDoubleRightCol = (short)(i_CurrentSlot.Position.Col + (short)i_AmountOfJump * (short)eDirectionMove.Right);
            short leftColOrDoubleLeftCol = (short)(i_CurrentSlot.Position.Col + (short)i_AmountOfJump * (short)eDirectionMove.Left);
            CheckersBoardSlot destinationSlotAfterToUpRight = r_GameBoard.GetCheckersBoardSlot(upRowOrDoubleUpRow, rightColOrDoubleRightCol);
            CheckersBoardSlot destinationSlotAfterToUpLeft = r_GameBoard.GetCheckersBoardSlot(upRowOrDoubleUpRow, leftColOrDoubleLeftCol);
            CheckersBoardSlot destinationSlotAfterToDownRight = r_GameBoard.GetCheckersBoardSlot(downRowOrDoubleDownRow, rightColOrDoubleRightCol);
            CheckersBoardSlot destinationSlotAfterToDownLeft = r_GameBoard.GetCheckersBoardSlot(downRowOrDoubleDownRow, leftColOrDoubleLeftCol);
            bool isCurrentSlotHasKing = i_CurrentSlot.Piece.PieceType == ePieceType.King;
            bool isThereValidMoveForFirstPlayer, isThereValidMoveForSecondPlayer;

            if(i_AmountOfJump == eDirectionMove.DoubleUpOrDoubleDown)
            {
                isThereValidMoveForFirstPlayer = checkIfThereIsValidEatingMoveForUpRightOrUpLeft(i_CurrentSlot, destinationSlotAfterToUpRight, destinationSlotAfterToUpLeft);
                isThereValidMoveForSecondPlayer = checkIfThereIsValidMoveForDownRightOrDownLeft(i_CurrentSlot, destinationSlotAfterToDownRight, destinationSlotAfterToDownLeft);
            }
            else
            {
                 isThereValidMoveForFirstPlayer =
                    checkIfFutureBoardSlotIsAvailable(destinationSlotAfterToUpRight)
                    || checkIfFutureBoardSlotIsAvailable(destinationSlotAfterToUpLeft);
                 isThereValidMoveForSecondPlayer = checkIfFutureBoardSlotIsAvailable(destinationSlotAfterToDownRight) || checkIfFutureBoardSlotIsAvailable(destinationSlotAfterToDownLeft);
            }

            addValidTurnsToComputerSlotListIfNeeded(
                i_ComputerValidTurnMoves,
                destinationSlotAfterToUpRight,
                destinationSlotAfterToUpLeft,
                destinationSlotAfterToDownRight,
                destinationSlotAfterToDownLeft,
                i_CurrentSlot,
                i_LastPlayedBoardSlot);

            return (i_IsFirstPlayer && isThereValidMoveForFirstPlayer)
                   || (!i_IsFirstPlayer && isThereValidMoveForSecondPlayer)
                   || isCurrentSlotHasKing && isThereValidMoveForFirstPlayer || isThereValidMoveForSecondPlayer;
        }

        private void addValidTurnsToComputerSlotListIfNeeded(LinkedList<CheckersPlayerTurn> i_ComputerValidTurnMoves, CheckersBoardSlot i_DestinationSlotAfterToUpRight,
                                                             CheckersBoardSlot i_DestinationSlotAfterToUpLeft, CheckersBoardSlot i_DestinationSlotAfterToDownRight,
                                                             CheckersBoardSlot i_DestinationSlotAfterToDownLeft, CheckersBoardSlot i_SourceSlot, CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            if (i_ComputerValidTurnMoves != null)
            { 
               addPlayerTurnToComputerTurnListIfValid(i_SourceSlot, i_DestinationSlotAfterToUpRight, i_ComputerValidTurnMoves, i_LastPlayedBoardSlot);
               addPlayerTurnToComputerTurnListIfValid(i_SourceSlot, i_DestinationSlotAfterToUpLeft, i_ComputerValidTurnMoves, i_LastPlayedBoardSlot);
               addPlayerTurnToComputerTurnListIfValid(i_SourceSlot, i_DestinationSlotAfterToDownRight, i_ComputerValidTurnMoves, i_LastPlayedBoardSlot);
               addPlayerTurnToComputerTurnListIfValid(i_SourceSlot, i_DestinationSlotAfterToDownLeft, i_ComputerValidTurnMoves, i_LastPlayedBoardSlot);
            }
        }

        private void addPlayerTurnToComputerTurnListIfValid(CheckersBoardSlot i_SourceSlot, CheckersBoardSlot i_FutureSlot, LinkedList<CheckersPlayerTurn> i_ComputerValidTurnMoves, CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            if(i_FutureSlot != null)
            {
                const ePlayerTurn k_ComputerTurn = ePlayerTurn.SecondPlayerTurn;
                CheckersPlayerTurn currentCheckersPlayerTurn = new CheckersPlayerTurn(i_SourceSlot, i_FutureSlot, k_ComputerTurn, r_GameBoard);

                addTurnToComputerSlotListIfValid(i_ComputerValidTurnMoves, currentCheckersPlayerTurn, i_LastPlayedBoardSlot);
            }
        }

        private void addTurnToComputerSlotListIfValid(LinkedList<CheckersPlayerTurn> i_ComputerValidTurnMoves, CheckersPlayerTurn i_TurnToAddIfValid, CheckersBoardSlot i_LastPlayedBoardSlot)
        {
            if(i_TurnToAddIfValid.CheckIfTurnIsValid(i_LastPlayedBoardSlot) == eMoveStatus.Valid)
            {
                i_ComputerValidTurnMoves.AddLast(i_TurnToAddIfValid);
            }
        }

        private bool checkIfThereIsValidEatingMoveForUpRightOrUpLeft(CheckersBoardSlot i_CurrentSlot, CheckersBoardSlot i_DestinationSlotAfterEatingToUpRight, CheckersBoardSlot i_DestinationSlotAfterEatingToUpLeft)
        {
            return (checkIfFutureBoardSlotIsAvailable(i_DestinationSlotAfterEatingToUpRight) && checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(i_CurrentSlot, i_DestinationSlotAfterEatingToUpRight, !v_UpdateEatingTargetSlotIfValid)) ||
                               (checkIfFutureBoardSlotIsAvailable(i_DestinationSlotAfterEatingToUpLeft) && checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(i_CurrentSlot, i_DestinationSlotAfterEatingToUpLeft, !v_UpdateEatingTargetSlotIfValid));
        }

        private bool checkIfThereIsValidMoveForDownRightOrDownLeft(CheckersBoardSlot i_CurrentSlot, CheckersBoardSlot i_DestinationSlotAfterEatingToDownRight, CheckersBoardSlot i_DestinationSlotAfterEatingToDownLeft)
        {
            return (checkIfFutureBoardSlotIsAvailable(i_DestinationSlotAfterEatingToDownRight) && checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(i_CurrentSlot, i_DestinationSlotAfterEatingToDownRight, !v_UpdateEatingTargetSlotIfValid)) ||
                               (checkIfFutureBoardSlotIsAvailable(i_DestinationSlotAfterEatingToDownLeft) && checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(i_CurrentSlot, i_DestinationSlotAfterEatingToDownLeft, !v_UpdateEatingTargetSlotIfValid));
        }

        private bool checkIfValidMoveAccordingToPieceType() 
        {
            BoardPosition sourcePosition = r_CurrentBoardSlot.Position;
            BoardPosition destinationPosition = r_FutureBoardSlot.Position;
            bool isRegularPiece = r_CurrentBoardSlot.Piece.PieceType == ePieceType.Regular;
            bool isKingPiece = r_CurrentBoardSlot.Piece.PieceType == ePieceType.King;
            bool firstPlayerIsPlayingAndTheCurrentPieceIsRegularAndTheMoveIsValid =
                r_IsFirstPlayerPlaying && isRegularPiece && destinationPosition.Row == (sourcePosition.Row + (short)eDirectionMove.Up);
            bool secondPlayerIsPlayingAndTheCurrentPieceIsRegularAndTheMoveIsValid =
                !r_IsFirstPlayerPlaying && isRegularPiece && destinationPosition.Row == (sourcePosition.Row + (short)eDirectionMove.Down);
            bool currentPieceIsKingAndTheMoveIsValidForKing = isKingPiece &&
                                                              (destinationPosition.Row == sourcePosition.Row + (short)eDirectionMove.Up
                                                               || destinationPosition.Row == (sourcePosition.Row + (short)eDirectionMove.Down));

            return firstPlayerIsPlayingAndTheCurrentPieceIsRegularAndTheMoveIsValid ||
                   secondPlayerIsPlayingAndTheCurrentPieceIsRegularAndTheMoveIsValid ||
                   currentPieceIsKingAndTheMoveIsValidForKing;
        }

        private bool checkIfFutureBoardSlotIsAvailable(CheckersBoardSlot i_FutureBoardSlot)
        {
            return i_FutureBoardSlot != null && (i_FutureBoardSlot.Piece.PlayerPieceOwner == ePlayerPieceOwner.None);
        }

        private bool checkIfThereIsPlayerPieceInCurrentBoardSlot()
        {
            return (r_CurrentPlayerPlaying == ePlayerTurn.FirstPlayerTurn
                                        && r_CurrentBoardSlot.Piece.PlayerPieceOwner == ePlayerPieceOwner.FirstPlayerPiece)
                                     || (r_CurrentPlayerPlaying == ePlayerTurn.SecondPlayerTurn
                                      && r_CurrentBoardSlot.Piece.PlayerPieceOwner == ePlayerPieceOwner.SecondPlayerPiece);
        }

        private bool checkAreSlotsAdjacent()
        {
            BoardPosition sourcePosition = r_CurrentBoardSlot.Position;
            BoardPosition destinationPosition = r_FutureBoardSlot.Position;
            bool adjacentSlots = false;

            if (destinationPosition.Row == sourcePosition.Row + (short)eDirectionMove.Down &&
                (destinationPosition.Col == sourcePosition.Col + (short)eDirectionMove.Right || destinationPosition.Col == sourcePosition.Col + (short)eDirectionMove.Left))
            {
                adjacentSlots = true;
            }
            else if (destinationPosition.Row == sourcePosition.Row + (short)eDirectionMove.Up
                    && (destinationPosition.Col == sourcePosition.Col + (short)eDirectionMove.Right || destinationPosition.Col == sourcePosition.Col + (short)eDirectionMove.Left))
            {
                adjacentSlots = true;
            }

            return adjacentSlots;
        }

        private bool checkIfValidMoveForEatingAndUpdateEatingTargetPositionIfValid(CheckersBoardSlot i_CurrentBoardSlot, CheckersBoardSlot i_FutureBoardSlot, bool i_UpdateIfValid)
        {
            bool validMoveForEat = false;

            if(i_CurrentBoardSlot != null && i_FutureBoardSlot != null)
            {
                BoardPosition sourcePosition = i_CurrentBoardSlot.Position;
                BoardPosition destinationPosition = i_FutureBoardSlot.Position;
                bool isEatingUp = checkIfEatingUp(sourcePosition, destinationPosition);
                bool isEatingDown = checkIfEatingDown(sourcePosition, destinationPosition);
                bool isCurrentBoardSlotHasKing = i_CurrentBoardSlot.Piece.PieceType == ePieceType.King;

                if(i_CurrentBoardSlot.Piece.PieceType != ePieceType.None
                   && (r_IsFirstPlayerPlaying && isEatingUp || (!r_IsFirstPlayerPlaying && isEatingDown)
                                                            || isCurrentBoardSlotHasKing
                                                            && (isEatingUp || isEatingDown)))
                {
                    validMoveForEat = checkIfValidEatingMoveAndUpdateTargetPositionIfValidAndAsked(
                        sourcePosition,
                        destinationPosition,
                        i_UpdateIfValid);
                }
            }

            return validMoveForEat;
        }

        private bool checkIfValidEatingMoveAndUpdateTargetPositionIfValidAndAsked(BoardPosition i_SourcePosition, BoardPosition i_DestinationPosition, bool i_UpdateIfValidAndAsked)
        {
            bool isCurrentSlotPieceHasKing = r_GameBoard.GetCheckersBoardSlot(i_SourcePosition.Row, i_SourcePosition.Col).Piece.PieceType == ePieceType.King;
            bool eatingUp = checkIfEatingUp(i_SourcePosition, i_DestinationPosition);
            bool eatingDown = checkIfEatingDown(i_SourcePosition, i_DestinationPosition);
            bool firstPlayerOrKingEatUp = (r_IsFirstPlayerPlaying || isCurrentSlotPieceHasKing) && eatingUp;
            bool secondPlayerOrKingEatDown = (!r_IsFirstPlayerPlaying || isCurrentSlotPieceHasKing) && eatingDown;
            ePieceTypeAndOwnershipInfoInSlot eatingPieceTypeAndOwnershipInfoInSlot = ePieceTypeAndOwnershipInfoInSlot.None;
            BoardPosition tempEatingTargetBoardPosition = null; 

            if (checkIfEatingToRight(i_SourcePosition, i_DestinationPosition)) 
            {
                if (firstPlayerOrKingEatUp)
                {
                    tempEatingTargetBoardPosition = new BoardPosition((short)(i_SourcePosition.Row + eDirectionMove.Up), (short)(i_SourcePosition.Col + eDirectionMove.Right));
                }
                else if (secondPlayerOrKingEatDown)
                {
                    tempEatingTargetBoardPosition = new BoardPosition((short)(i_SourcePosition.Row + eDirectionMove.Down), (short)(i_SourcePosition.Col + eDirectionMove.Right));
                }
            }
            else if (checkIfEatingToLeft(i_SourcePosition, i_DestinationPosition))
            {
                if (firstPlayerOrKingEatUp)
                {
                    tempEatingTargetBoardPosition = new BoardPosition((short)(i_SourcePosition.Row + eDirectionMove.Up), (short)(i_SourcePosition.Col + eDirectionMove.Left));
                }
                else if (secondPlayerOrKingEatDown)
                {
                    tempEatingTargetBoardPosition = new BoardPosition((short)(i_SourcePosition.Row + eDirectionMove.Down), (short)(i_SourcePosition.Col + eDirectionMove.Left));
                }
            }

            eatingPieceTypeAndOwnershipInfoInSlot = r_GameBoard.GetPieceOwnershipAndPieceTypeAccordingToBoardPosition(tempEatingTargetBoardPosition);
            
            bool firstPlayerIsPlayingAndEatingSecondPlayer = r_IsFirstPlayerPlaying && (eatingPieceTypeAndOwnershipInfoInSlot
                                                                 == ePieceTypeAndOwnershipInfoInSlot
                                                                     .SecondPlayerKingPiece
                                                                 || eatingPieceTypeAndOwnershipInfoInSlot
                                                                 == ePieceTypeAndOwnershipInfoInSlot
                                                                     .SecondPlayerPiece);
            bool secondPlayerIsPlayingAndEatingFirstPlayer = !r_IsFirstPlayerPlaying
                                                             && (eatingPieceTypeAndOwnershipInfoInSlot
                                                                 == ePieceTypeAndOwnershipInfoInSlot
                                                                     .FirstPlayerKingPiece
                                                                 || eatingPieceTypeAndOwnershipInfoInSlot
                                                                 == ePieceTypeAndOwnershipInfoInSlot
                                                                     .FirstPlayerPiece);

            if (i_UpdateIfValidAndAsked)
            {
                m_EatingTargetBoardPosition = tempEatingTargetBoardPosition;
            }

            return firstPlayerIsPlayingAndEatingSecondPlayer || secondPlayerIsPlayingAndEatingFirstPlayer;
        }

        private bool checkIfEatingToRight(BoardPosition i_SourcePosition, BoardPosition i_DestinationPosition)
        {
            return i_SourcePosition.Col + (2 * (short)eDirectionMove.Right) == i_DestinationPosition.Col;
        }

        private bool checkIfEatingToLeft(BoardPosition i_SourcePosition, BoardPosition i_DestinationPosition)
        {
            return i_SourcePosition.Col + (2 * (short)eDirectionMove.Left) == i_DestinationPosition.Col;
        }

        private bool checkIfEatingUp(BoardPosition i_SourcePosition, BoardPosition i_DestinationPosition)
        {
            return i_SourcePosition.Row + (2 * (short)eDirectionMove.Up) == i_DestinationPosition.Row;
        }

        private bool checkIfEatingDown(BoardPosition i_SourcePosition, BoardPosition i_DestinationPosition)
        {
            return i_SourcePosition.Row + (2 * (short)eDirectionMove.Down) == i_DestinationPosition.Row;
        }
    }
}
