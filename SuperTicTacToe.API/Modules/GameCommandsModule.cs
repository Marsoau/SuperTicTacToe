using SuperTicTacToe.API.Attributes;
using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Model;
using SuperTicTacToe.API.Model.Game;

namespace SuperTicTacToe.API.Modules
{
    public class GameCommandsModule
    {
        private Player? _invoker;
        public Player Invoker {
            get => _invoker ?? throw new Exception("Command required player was not found");
            set => _invoker = value;
        }
        public GameRoom Room {
            get => Invoker.Room;
        }

        public GameCommandsModule(Player invoker) {
            _invoker = invoker;
        }

        [GameCommand]
        public bool PlaceAt(int gameX, int gameY, int x, int y) {
            if (Room.CurrentTurnChar == TTTChar.X && Room.PlayerX != Invoker) return false;
            if (Room.CurrentTurnChar == TTTChar.O && Room.PlayerO != Invoker) return false;
            return Invoker.PlaceAt(gameX, gameY, x, y);
        }

        [GameCommand]
        public void SwitchToX() {
            if (Room.PlayerX is null) Room.PlayerX = Invoker;
        }
        [GameCommand]
        public void SwitchToO() {
            if (Room.PlayerO is null) Room.PlayerO = Invoker;
        }
        [GameCommand]
        public void SwitchToSpectator() {
            if (Room.PlayerX == Invoker) Room.PlayerX = null;
            else if (Room.PlayerO == Invoker) Room.PlayerO = null;
        }
    }
}
