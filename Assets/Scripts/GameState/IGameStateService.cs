using System;

namespace Test.Game2048.GameState
{
    public interface IGameStateService
    {
        event Action GameOver;
        event Action<bool> Changed;
        bool IsGameOver { get; }
        void TriggerGameOver();
        void Reset();
    }
}

