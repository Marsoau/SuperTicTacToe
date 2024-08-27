using SuperTicTacToe.API.Enums;
using SuperTicTacToe.API.Interfaces;
using System.Text.Json.Serialization;

namespace SuperTicTacToe.API.Model.Game
{
    public abstract class GameBase : IHasResult
    {
        public event Action? OnFinalResultChanged;

        protected abstract TTTResult[] _resultTable { get; }
        private TTTResult _finalResult;
        [JsonPropertyName("finalResult")]
        public TTTResult FinalResult {
            get => _finalResult;
            set {
                if (_finalResult == value) return;

                _finalResult = value;

                OnFinalResultChanged?.Invoke();

                Console.WriteLine($"new final result for [{this}]: {FinalResult}");
            }
        }

        private TTTResult GetAt(int x, int y) {
            return _resultTable[x + y * 3];
        }

        public TTTResult UpdateResult() {
            if (FinalResult != TTTResult.None)
                return FinalResult;
            if (_resultTable.Length != 9)
                throw new Exception("invalid table size");

            var noEmptySpace = true;
            foreach (var result in _resultTable) {
                if (result == TTTResult.None) {
                    noEmptySpace = false;
                    break;
                }
            }
            if (noEmptySpace) return FinalResult = TTTResult.NoOne;

            for (int i = 0; i < 3; i++) {
                if (_resultTable[i * 3 + 0] != TTTResult.None &&
                    _resultTable[i * 3 + 0] != TTTResult.NoOne &&
                    _resultTable[i * 3 + 0] == _resultTable[i * 3 + 1] &&
                    _resultTable[i * 3 + 1] == _resultTable[i * 3 + 2]
                ) {
                    return FinalResult = _resultTable[i * 3 + 0];
                }
                if (_resultTable[0 * 3 + i] != TTTResult.None &&
                    _resultTable[0 * 3 + i] != TTTResult.NoOne &&
                    _resultTable[0 * 3 + i] == _resultTable[1 * 3 + i] &&
                    _resultTable[1 * 3 + i] == _resultTable[2 * 3 + i]
                ) {
                    return FinalResult = _resultTable[0 * 3 + i];
                }
            }
            if (_resultTable[0 * 3 + 0] != TTTResult.None &&
                _resultTable[0 * 3 + 0] != TTTResult.NoOne &&
                _resultTable[0 * 3 + 0] == _resultTable[1 * 3 + 1] &&
                _resultTable[1 * 3 + 1] == _resultTable[2 * 3 + 2]
            ) {
                return FinalResult = _resultTable[0 * 3 + 0];
            }
            if (_resultTable[0 * 3 + 2] != TTTResult.None &&
                _resultTable[0 * 3 + 2] != TTTResult.NoOne &&
                _resultTable[0 * 3 + 2] == _resultTable[1 * 3 + 1] &&
                _resultTable[1 * 3 + 1] == _resultTable[2 * 3 + 0]
            ) {
                return FinalResult = _resultTable[0 * 3 + 2];
            }

            return FinalResult = TTTResult.None;
        }
    }
}
