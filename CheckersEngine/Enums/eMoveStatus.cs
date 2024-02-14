namespace CheckersEngine.Enums
{
    public enum eMoveStatus
    {
        MoveHasNotCheckedYet,
        Valid,
        ErrorOccurredDuringMove,
        DoNotHavePlayerPieceInSlot,
        NotAllowedSlot,
        SlotsAreNotAdjacent,
        FutureBoardSlotIsNotAvailable,
        InvalidMoveAccordingToPieceType,
        MustDoEatingMove,
        HaveAnotherEatingMoveWithTheLastPiecePlayed,
    }
}
