using System;

namespace Test.Game2048.Score
{
    public interface IScoreService
    {
        event Action<int> Changed;
        int Current { get; }
        void AddMergeReward(int mergedCubeValue);
        void Reset();
    }
}

