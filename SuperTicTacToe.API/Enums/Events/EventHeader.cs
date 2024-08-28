namespace SuperTicTacToe.API.Enums.Events
{
    public enum EventHeader
    {
        AvailableGamesChanged,

        XPlayerChanged,
        OPlayerChanged,
        SpectatorPlayersChanged,
        SuperGameResultChanged,
        MiniGameResultChanged,
        MiniGameStateChanged,
        MiniGameCellWasSet,
        TurnCharChanged,
        XScoreChanged,
        OScoreChanged,
        PlayerNameChanged,
        PlayerScoreChanged,
        GameEnded
    }
}
