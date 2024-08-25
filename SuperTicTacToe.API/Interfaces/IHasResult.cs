using SuperTicTacToe.API.Enums;

namespace SuperTicTacToe.API.Interfaces
{
    public interface IHasResult
    {
        TTTResult Result {
            get => GetResult();
        }
        TTTResult GetResult();
    }
}
