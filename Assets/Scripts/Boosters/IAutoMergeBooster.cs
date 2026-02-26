using Cysharp.Threading.Tasks;

namespace Test.Game2048.Boosters
{
    public interface IAutoMergeBooster
    {
        bool IsBusy { get; }
        UniTask<bool> TryExecuteAsync();
    }
}

