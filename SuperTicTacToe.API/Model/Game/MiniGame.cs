using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Interfaces;

namespace SuperTicTacToe.API.Model.Game
{
    public class MiniGame : GameBase
    {
        private TTTChar?[] _table;
        protected override IHasResult[] _resultTable => throw new NotImplementedException();

        public MiniGame() {

        }
    }
}
