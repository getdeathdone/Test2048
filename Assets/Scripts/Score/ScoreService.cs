using System;

namespace Test.Game2048.Score
{
    public class ScoreService : IScoreService
    {
        public event Action<int> Changed;

        public int Current { get; private set; }

        public void AddMergeReward(int mergedCubeValue)
        {
            var reward = Math.Max(1, mergedCubeValue / 4);
            Current += reward;
            Changed?.Invoke(Current);
        }

        public void Reset()
        {
            Current = 0;
            Changed?.Invoke(Current);
        }
    }
}

