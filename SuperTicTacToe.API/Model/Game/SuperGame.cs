using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Interfaces;
using System.Text;
using System.Text.Json.Serialization;

namespace SuperTicTacToe.API.Model.Game
{
    public class SuperGame : GameBase
    {
        [JsonPropertyName("miniGames")]
        public MiniGame[] MiniGames { get; }
        protected override TTTResult[] _resultTable
            => MiniGames.Select(minigame => minigame.FinalResult).ToArray();

        public SuperGame() {
            MiniGames = new MiniGame[9];

            for (int i = 0; i < MiniGames.Length; i++) {
                MiniGames[i] = new MiniGame();
            }
        }

        public void Clear() {
            foreach (var game in MiniGames) {
                game.Clear();
            }
            FinalResult = TTTResult.None;

            //Send events
        }

        public bool PlaceAt(int gameX, int gameY, int posX, int posY, TTTChar c) {
            var game = MiniGames[gameX + gameY * 3];
            bool placementResult = game.PlaceAt(posX, posY, c);
            if (!placementResult) return false;
            
            for (int y = 0; y < 3; y++) {
                for (int x = 0; x < 3; x++) {
                    MiniGames[x + y * 3].IsEnabled = game.FinalResult != TTTResult.None || (x == posX && y == posY);
                }
            }

            UpdateResult();

            //send events
            return true;
        }
    }
}
