using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Test.Game2048.Cubes;
using Test.Game2048.Cubes.Abstractions;
using Test.Game2048.GameState;
using Test.Game2048.Score;
using Zenject;
using Object = UnityEngine.Object;

namespace Test.Game2048.Gameplay
{
    public class GameFlow : IInitializable, IDisposable, IGameRestartService
    {
        private readonly ICubeSpawner _spawner;
        private readonly ICubeSling _sling;
        private readonly ICubeCombiner _combiner;
        private readonly IScoreService _scoreService;
        private readonly IGameStateService _gameState;
        private readonly GameRulesData _rules;
        private CancellationTokenSource _combineCts;

        public GameFlow(
            ICubeSpawner spawner,
            ICubeSling sling,
            ICubeCombiner combiner,
            IScoreService scoreService,
            IGameStateService gameState,
            GameRulesData rules)
        {
            _spawner = spawner;
            _sling = sling;
            _combiner = combiner;
            _scoreService = scoreService;
            _gameState = gameState;
            _rules = rules;
        }

        public void Initialize()
        {
            _combineCts = new CancellationTokenSource();
            _sling.Detached += OnCubeDetach;
            _combiner.Combined += OnCubeCombined;
            SpawnNewCube();
        }

        public void Dispose()
        {
            _sling.Detached -= OnCubeDetach;
            _combiner.Combined -= OnCubeCombined;
            _sling.Reset();
            ResetCombineToken();
        }

        private void OnCubeDetach(Cube cube)
        {
            cube.Collide += OnCubeCollide;
            SpawnNewCube();
        }

        private void SpawnNewCube()
        {
            if (_gameState.IsGameOver)
                return;

            if (GetDroppedCubeCount() >= _rules.MaxCubesOnBoard)
            {
                _gameState.TriggerGameOver();
                return;
            }

            var cube = _spawner.SpawnRandom();
            _sling.Attach(cube);
        }

        private void OnCubeCollide(Cube cube1, Cube cube2, float directedSpeed)
        {
            if (_combiner.CanCombine(cube1, cube2, directedSpeed) == false)
                return;

            cube1.Collide -= OnCubeCollide;
            cube2.Collide -= OnCubeCollide;

            CombineAsync(cube1, cube2, _combineCts.Token).Forget();
        }

        private async UniTaskVoid CombineAsync(Cube cube1, Cube cube2, CancellationToken cancellationToken)
        {
            try
            {
                var mergedCube = await _combiner.CombineAsync(cube1, cube2, applyPostImpact: true, cancellationToken);
                if (mergedCube != null)
                    _scoreService.AddMergeReward(mergedCube.Number);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnCubeCombined(Cube cube)
        {
            cube.Collide += OnCubeCollide;
        }

        private static int GetDroppedCubeCount()
        {
            var dropped = 0;

            foreach (var cube in Cube.ActiveCubes)
            {
                if (cube.IsKinematic == false)
                    dropped++;
            }

            return dropped;
        }

        public void Restart()
        {
            var cubes = new List<Cube>(Cube.ActiveCubes);

            ResetCombineToken();
            _combineCts = new CancellationTokenSource();

            _sling.Reset();
            _scoreService.Reset();
            _gameState.Reset();

            foreach (var cube in cubes)
            {
                if (cube == null)
                    continue;

                cube.Collide -= OnCubeCollide;
                Object.Destroy(cube.gameObject);
            }

            SpawnNewCube();
        }

        private void ResetCombineToken()
        {
            if (_combineCts == null)
                return;

            _combineCts.Cancel();
            _combineCts.Dispose();
            _combineCts = null;
        }
    }
}
