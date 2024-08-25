using SuperTicTacToe.API.Interfaces;

namespace SuperTicTacToe.API.Model.Game
{
    public class SuperGame : GameBase
    {
        protected override IHasResult[] _resultTable => throw new NotImplementedException();
    }
}
