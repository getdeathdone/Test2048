using System;

namespace Test.Game2048.GameState
{
    public class GameStateService : IGameStateService
    {
        public event Action GameOver;
        public event Action<bool> Changed;
        public bool IsGameOver { get; private set; }

        public void TriggerGameOver()
        {
            if (IsGameOver)
                return;

            IsGameOver = true;
            Changed?.Invoke(true);
            GameOver?.Invoke();
        }

        public void Reset()
        {
            if (IsGameOver == false)
                return;

            IsGameOver = false;
            Changed?.Invoke(false);
        }
    }
}

