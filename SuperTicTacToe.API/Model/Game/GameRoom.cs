using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Enums.Events;
using SuperTicTacToe.API.Modules;
using SuperTicTacToe.API.Services;
using System.Text.Json.Serialization;

namespace SuperTicTacToe.API.Model.Game
{
    public class GameRoom
    {
        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("name")]
        public string Name { get; }
        [JsonIgnore]
        public string? Password { get; }

        [JsonPropertyName("spectators")]
        public List<Player> SpectatorPlayers { get; }
        [JsonIgnore]
        private Player? _playerX;
        [JsonIgnore]
        private Player? _playerO;

        [JsonPropertyName("playerX")]
        public Player? PlayerX {
            get => _playerX;
            set {
                if (PlayerO == value) PlayerO = null;

                if (_playerX is not null) {
                    SpectatorPlayers.Add(_playerX);
                    _playerX.Char = null;

                    Events.SendEvent(EventHeader.SpectatorPlayersChanged, SpectatorPlayers);
                }
                if (value is not null) {
                    SpectatorPlayers.Remove(value);
                    _playerX = value;
                    _playerX.Char = TTTChar.X;
                }
                else _playerX = value;
                
                Events.SendEvent(EventHeader.XPlayerChanged, PlayerX);
            }
        }

        [JsonPropertyName("playerO")]
        public Player? PlayerO {
            get => _playerO;
            set {
                if (PlayerX == value) PlayerX = null;

                if (_playerO is not null) {
                    SpectatorPlayers.Add(_playerO);
                    _playerO.Char = null;

                    Events.SendEvent(EventHeader.SpectatorPlayersChanged, SpectatorPlayers);
                }
                if (value is not null) {
                    SpectatorPlayers.Remove(value);
                    _playerO = value;
                    _playerO.Char = TTTChar.O;
                }
                else _playerO = value;

                Events.SendEvent(EventHeader.OPlayerChanged, PlayerO);
            }
        }

        [JsonPropertyName("superGame")]
        public SuperGame Game { get; }
        public readonly EventsService Events;

        [JsonPropertyName("currentTurnChar")]
        public TTTChar CurrentTurnChar {
            get => _currentTurnChar;
            set {
                _currentTurnChar = value;

                Events.SendEvent(EventHeader.TurnCharChanged, CurrentTurnChar);
            }
        }
        private TTTChar _currentTurnChar;

        private static int _idCounter = 0;
        public GameRoom(string name, string? password = null) {
            Id = _idCounter++;

            Name = name;
            Password = password;

            Events = new EventsService();
            Game = new SuperGame();

            SpectatorPlayers = new List<Player>();

            _currentTurnChar = TTTChar.X;

            SubscribeToEvents();
        }

        private void SubscribeToEvents() {
            Game.OnFinalResultChanged += () => {
                Events.SendEvent(EventHeader.SuperGameResultChanged, Game.FinalResult);
            };
            for (int i = 0; i < 9; i++) {
                var exIndex = i;
                Game.MiniGames[exIndex].OnFinalResultChanged += () => {
                    Events.SendEvent(EventHeader.MiniGameResultChanged, new {
                        gameI = exIndex,
                        newResult = Game.MiniGames[exIndex].FinalResult
                    });
                };
                Game.MiniGames[exIndex].OnCellChanged += (x, y, c) => {
                    Events.SendEvent(EventHeader.MiniGameCellWasSet, new {
                        gameI = exIndex,
                        x,
                        y,
                        c
                    });
                };
                Game.MiniGames[exIndex].OnStateChanged += (newState) => {
                    Events.SendEvent(EventHeader.MiniGameStateChanged, new {
                        gameI = exIndex,
                        newState
                    });
                };
            }
        }

        public void Stop() {
            throw new NotImplementedException();
        }

        public Player AddPlayer() {
            var player = new Player(this, null, TTTChar.X);

            SpectatorPlayers.Add(player);

            Events.SendEvent(EventHeader.SpectatorPlayersChanged, SpectatorPlayers);

            return player;
        }
        public void PlayerLeave(Guid playerToken)
            => GetRequiredPlayer(playerToken);
        public void PlayerLeave(Player player) {
            SpectatorPlayers.Remove(player);

            if (PlayerX == player)
                PlayerX = null;
            if (PlayerO == player)
                PlayerO = null;

            Events.SendEvent(EventHeader.SpectatorPlayersChanged, SpectatorPlayers);
        }

        public Player GetRequiredPlayer(Guid token)
            => GetPlayer(token) ?? throw new Exception($"Player with token \"{token}\" was not found");
        public Player? GetPlayer(Guid token) {
            return SpectatorPlayers.FirstOrDefault(player => player.Token == token)
                ?? (PlayerX?.Token == token ? PlayerX : null)
                ?? (PlayerO?.Token == token ? PlayerO : null);
        }

        public TTTChar SwitchTurn() {
            if (CurrentTurnChar == TTTChar.X) return CurrentTurnChar = TTTChar.O;
            if (CurrentTurnChar == TTTChar.O) return CurrentTurnChar = TTTChar.X;
            throw new Exception("imposible exception");
        }
    }
}
