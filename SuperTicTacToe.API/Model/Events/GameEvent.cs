using SuperTicTacToe.API.Enums.Events;
using System.Text.Json;

namespace SuperTicTacToe.API.Model.Events
{
    public class GameEvent
    {
        public readonly GameEventHeader Header;
        public object? data;

        public int GameRoomId;

        public GameEvent(int gameRoomId, GameEventHeader header) {
            GameRoomId = gameRoomId;
            Header = header;
        }
        public GameEvent(int gameRoomId, GameEventHeader header, object? data) : this(gameRoomId, header) {
            this.data = data;
        }

        public override string ToString() {
            return $"[{GameRoomId}] {{{Header}}}: {data}";
        }
        public string ToSSEString() {
            return $"event: {Header}\rdata: {JsonSerializer.Serialize(new {
                gameRoomId = GameRoomId,
                data
            })}\r\r";
        }
    }
}
