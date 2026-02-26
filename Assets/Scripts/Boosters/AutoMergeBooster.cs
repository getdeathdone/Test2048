using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Test.Game2048.Cubes;
using Test.Game2048.Cubes.Abstractions;
using UnityEngine;

namespace Test.Game2048.Boosters
{
    public class AutoMergeBooster : IAutoMergeBooster
    {
        private readonly ICubeCombiner _combiner;
        private readonly AutoMergeBoosterData _data;

        public bool IsBusy { get; private set; }

        public AutoMergeBooster(ICubeCombiner combiner, AutoMergeBoosterData data)
        {
            _combiner = combiner;
            _data = data;
        }

        public async UniTask<bool> TryExecuteAsync()
        {
            if (IsBusy)
                return false;

            if (TryFindPair(out var cube1, out var cube2) == false)
                return false;

            IsBusy = true;
            try
            {
                await PlayPreMergeAnimation(cube1, cube2);
                var mergedCube = await _combiner.CombineAsync(cube1, cube2, applyPostImpact: false);
                SpawnParticles(mergedCube);
                return mergedCube != null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool TryFindPair(out Cube cube1, out Cube cube2)
        {
            cube1 = null;
            cube2 = null;

            var cubes = Cube.ActiveCubes
                .Where(c => c != null && c.IsKinematic == false)
                .OrderBy(c => c.Number)
                .ThenBy(c => c.transform.position.z)
                .ToArray();

            for (var i = 0; i < cubes.Length; i++)
            {
                for (var j = i + 1; j < cubes.Length; j++)
                {
                    if (cubes[i].Number != cubes[j].Number)
                        break;

                    cube1 = cubes[i];
                    cube2 = cubes[j];
                    return true;
                }
            }

            return false;
        }

        private async UniTask PlayPreMergeAnimation(Cube cube1, Cube cube2)
        {
            cube1.EnableKinematic();
            cube2.EnableKinematic();

            var riseOffset = Vector3.up * _data.RiseHeight;
            var riseSeq = DOTween.Sequence()
                .Join(cube1.transform.DOMove(cube1.transform.position + riseOffset, _data.RiseDuration))
                .Join(cube2.transform.DOMove(cube2.transform.position + riseOffset, _data.RiseDuration));
            await AwaitTween(riseSeq);

            var middle = (cube1.transform.position + cube2.transform.position) * 0.5f;
            var dir1 = (cube1.transform.position - middle).normalized;
            var dir2 = (cube2.transform.position - middle).normalized;

            var swingSeq = DOTween.Sequence()
                .Join(cube1.transform.DOMove(cube1.transform.position + dir1 * _data.SwingBackDistance, _data.SwingBackDuration))
                .Join(cube2.transform.DOMove(cube2.transform.position + dir2 * _data.SwingBackDistance, _data.SwingBackDuration));
            await AwaitTween(swingSeq);

            var flySeq = DOTween.Sequence()
                .Join(cube1.transform.DOMove(middle, _data.MergeFlightDuration))
                .Join(cube2.transform.DOMove(middle, _data.MergeFlightDuration));
            await AwaitTween(flySeq);
        }

        private void SpawnParticles(Cube mergedCube)
        {
            if (_data.MergeParticles == null || mergedCube == null)
                return;

            Object.Instantiate(_data.MergeParticles, mergedCube.transform.position, Quaternion.identity);
        }

        private static UniTask AwaitTween(Tween tween)
        {
            var completion = new UniTaskCompletionSource();
            tween.OnComplete(() => completion.TrySetResult());
            tween.OnKill(() => completion.TrySetCanceled());
            return completion.Task;
        }
    }
}

