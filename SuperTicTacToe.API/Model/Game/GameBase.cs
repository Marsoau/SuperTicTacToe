using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Interfaces;

namespace SuperTicTacToe.API.Model.Game
{
    public abstract class GameBase : IHasResult
    {
        protected abstract IHasResult[] _resultTable { get; }

        public TTTResult GetResult() {
            if (_resultTable.Length != 9)
                throw new Exception("invalid table size");

            return TTTResult.Noone;
        }
    }
}
