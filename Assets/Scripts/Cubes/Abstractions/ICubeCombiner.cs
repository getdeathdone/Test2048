using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Test.Game2048.Cubes.Abstractions
{
    public interface ICubeCombiner
    {
        event Action<Cube> Combined;
        bool CanCombine(Cube cube1, Cube cube2, float directedSpeed);
        UniTask<Cube> CombineAsync(Cube cube1, Cube cube2, bool applyPostImpact, CancellationToken cancellationToken = default);
    }
}

