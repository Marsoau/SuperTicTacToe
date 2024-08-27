using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Interfaces;
using System.Text.Json.Serialization;

namespace SuperTicTacToe.API.Model.Game
{
    public class MiniGame : GameBase
    {
        [JsonPropertyName("table")]
        public TTTChar?[] Table { get; }
        protected override TTTResult[] _resultTable {
            get => Table.Select(cell => cell.HasValue ? (TTTResult)cell : TTTResult.None).ToArray();
        }

        public event Action<int, int, TTTChar>? OnCellChanged;

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        public MiniGame() {
            Table = new TTTChar?[9];
            IsEnabled = true;
        }

        public void Clear() {
            for (int i = 0; i < Table.Length; i++) {
                Table[i] = null;
            }
            FinalResult = TTTResult.None;
        }

        public TTTChar? GetAt(int x, int y) {
            return Table[x + y * 3];
        }
        public void SetAt(int x, int y, TTTChar c) {
            Table[x + y * 3] = c;
            OnCellChanged?.Invoke(x, y, c);
        }

        public bool PlaceAt(int x, int y, TTTChar c) {
            if (FinalResult != TTTResult.None) return false;
            if (!IsEnabled) return false;

            if (GetAt(x, y) is null) {
                SetAt(x, y, c);
                UpdateResult();
            } else return false;

            return true;
        }
    }
}
