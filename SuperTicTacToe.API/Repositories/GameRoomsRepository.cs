using SuperTicTacToe.API.Enums.Events;
using SuperTicTacToe.API.Model;
using SuperTicTacToe.API.Model.Game;
using SuperTicTacToe.API.Services;

namespace SuperTicTacToe.API.Repositories
{
    public class GameRoomsRepository
    {
        public readonly List<GameRoom> Rooms;
        public GameRoomsRepository() {
            Events = new EventsService();
            Rooms = new List<GameRoom>();
        }

        public readonly EventsService Events;

        public GameRoom Create(string gameName, string? password = null) {
            var room = new GameRoom(gameName, password);

            Rooms.Add(room);
            SendGamesListUpdate();

            return room;
        }

        public GameRoom GetRequired(int id)
            => Get(id) ?? throw new Exception($"Room with id \"{id}\" not found");
        public GameRoom? Get(int id) {
            return Rooms.FirstOrDefault(room => room.Id == id);
        }

        public Player GetRequiredGamePlayer(Guid token)
            => GetGamePlayer(token) ?? throw new Exception($"Player with token \"{token}\" was not found");
        public Player? GetGamePlayer(Guid token) {
            Player? player;
            foreach (var room in Rooms) {
                player = room.GetPlayer(token);
                if (player is not null) return player;
            }

            return null;
        }

        private void SendGamesListUpdate() {
            Events.SendEvent(EventHeader.AvailableGamesChanged, Rooms.Select(room => new {
                id = room.Id,
                name = room.Name,
                hasPassword = room.Password is not null,
            }));
        }
    }
}
