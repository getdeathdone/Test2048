using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Test.Game2048.Config;
using Test.Game2048.Cubes.Abstractions;
using Test.Game2048.Cubes.Extensions;
using UnityEngine;

namespace Test.Game2048.Cubes
{
    public class CubeCombiner : ICubeCombiner
    {
        public event Action<Cube> Combined;

        private readonly CubeCombinerData _data;
        private readonly HashSet<int> _combiningCubes = new HashSet<int>();

        public CubeCombiner(CubeCombinerData data)
        {
            _data = data;
        }

        public bool CanCombine(Cube cube1, Cube cube2, float directedSpeed)
        {
            if (cube1 == null || cube2 == null)
                return false;

            if (cube1 == cube2)
                return false;

            if (cube1.Number != cube2.Number)
                return false;

            if (directedSpeed < _data.MinDirectedSpeed)
                return false;

            return _combiningCubes.Contains(cube1.GetInstanceID()) == false
                   && _combiningCubes.Contains(cube2.GetInstanceID()) == false;
        }

        public async UniTask<Cube> CombineAsync(Cube cube1, Cube cube2, bool applyPostImpact, CancellationToken cancellationToken = default)
        {
            if (CanLock(cube1, cube2) == false)
                return null;

            try
            {
                if (cube1 == null || cube2 == null)
                    return null;

                var nextNumber = GetNextNumber(cube1);
                var nextColor = GetNextColor(nextNumber);

                var middlePosition = GetMiddlePosition(cube1, cube2);
                var rotation = GetRotation(cube1);
                var duration = _data.CombineDuration;

                cube1.EnableKinematic();
                cube2.EnableKinematic();

                var sequence = DOTween.Sequence()
                    .Join(cube1.DOMove(middlePosition, duration))
                    .Join(cube2.DOMove(middlePosition, duration))
                    .Join(cube2.DORotate(rotation, duration))
                    .Join(cube1.DOColor(nextColor, duration))
                    .Join(cube2.DOColor(nextColor, duration))
                    .Join(cube1.DONumber(nextNumber, duration))
                    .Join(cube2.DONumber(nextNumber, duration));

                await AwaitSequence(sequence, cancellationToken);

                if (cube1 == null || cube2 == null)
                    return null;

                cube1.DisableKinematic();
                UnityEngine.Object.Destroy(cube2.gameObject);

                Combined?.Invoke(cube1);

                if (applyPostImpact)
                {
                    cube1.Push(Vector3.up, _data.PushUpForce);
                    cube1.Rotate(UnityEngine.Random.rotation.eulerAngles);
                }

                return cube1;
            }
            finally
            {
                Unlock(cube1, cube2);
            }
        }

        private bool CanLock(Cube cube1, Cube cube2)
        {
            if (cube1 == null || cube2 == null)
                return false;

            var id1 = cube1.GetInstanceID();
            var id2 = cube2.GetInstanceID();

            if (_combiningCubes.Contains(id1) || _combiningCubes.Contains(id2))
                return false;

            _combiningCubes.Add(id1);
            _combiningCubes.Add(id2);
            return true;
        }

        private void Unlock(Cube cube1, Cube cube2)
        {
            if (cube1 != null)
                _combiningCubes.Remove(cube1.GetInstanceID());

            if (cube2 != null)
                _combiningCubes.Remove(cube2.GetInstanceID());
        }

        private static UniTask AwaitSequence(Sequence sequence, CancellationToken cancellationToken)
        {
            var completion = new UniTaskCompletionSource();

            sequence.OnComplete(() => completion.TrySetResult());
            sequence.OnKill(() => completion.TrySetCanceled());

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    if (sequence.IsActive())
                        sequence.Kill();
                });
            }

            return completion.Task;
        }

        private static Vector3 GetMiddlePosition(Cube cube1, Cube cube2)
        {
            var position1 = cube1.transform.position;
            var position2 = cube2.transform.position;
            return position2 + ((position1 - position2) / 2f);
        }

        private static Vector3 GetRotation(Cube cube)
        {
            return cube.transform.rotation.eulerAngles;
        }

        private Color32 GetNextColor(int number)
        {
            var nextIndex = _data.NumberGenerator.GetIndex(number);
            return _data.Colors.GetColor(nextIndex);
        }

        private int GetNextNumber(Cube cube)
        {
            return _data.NumberGenerator.GenerateNext(cube.Number);
        }
    }
}
