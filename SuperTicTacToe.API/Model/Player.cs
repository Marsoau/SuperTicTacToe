using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Enums.Events;
using SuperTicTacToe.API.Model.Game;
using SuperTicTacToe.API.Modules;
using System.Text.Json.Serialization;

namespace SuperTicTacToe.API.Model
{
    public class Player
    {
        [JsonIgnore]
        public readonly Guid Token;
        [JsonIgnore]
        public readonly GameRoom Room;

        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("name")]
        public string Name {
            get => _name;
            set {
                _name = value;

                Room.Events.SendEvent(EventHeader.PlayerNameChanged, Name);
            }
        }
        private string _name;

        [JsonPropertyName("score")]
        public int Score {
            get => _score;
            set {
                _score = value;

                Room.Events.SendEvent(EventHeader.PlayerScoreChanged, Score);
            }
        }
        private int _score;

        [JsonIgnore]
        public TTTChar? Char { get; set; }
        [JsonIgnore]
        public GameCommandsModule Commands
            => new GameCommandsModule(this);

        private static int _idCounter = 0;
        public Player(GameRoom parentRoom, string? name = null, TTTChar? c = null) {
            Token = Guid.NewGuid();

            Id = _idCounter++;
            _name = name ?? $"Player - {Id}";

            Room = parentRoom;
            Char = c;
        }

        public bool PlaceAt(int gameX, int gameY, int x, int y) {
            if (Char is null) return false;

            bool placementResult = Room.Game.PlaceAt(gameX, gameY, x, y, Char.Value);
            if (!placementResult) return false;

            Room.SwitchTurn();

            return true;
        }
    }
}
