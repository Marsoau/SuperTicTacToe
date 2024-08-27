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
        MiniGameCellWasSet,
        TurnCharChanged,
        XScoreChanged,
        OScoreChanged,
        PlayerNameChanged,
        PlayerScoreChanged,
        GameEnded
    }
}
