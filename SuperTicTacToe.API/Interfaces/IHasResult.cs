using SuperTicTacToe.API.Enums;

namespace SuperTicTacToe.API.Interfaces
{
    public interface IHasResult
    {
        public TTTResult Result {
            get => UpdateResult();
        }
        public TTTResult UpdateResult();
    }
}
