using Test.Game2048.Boosters;
using Test.Game2048.Config;
using Test.Game2048.Cubes;
using Test.Game2048.Cubes.Abstractions;
using Test.Game2048.Gameplay;
using Test.Game2048.GameState;
using Test.Game2048.Input;
using Test.Game2048.Score;
using System;
using UnityEngine;
using Zenject;

namespace Test.Game2048.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CubeSpawnerData _cubeSpawnerData;
        [SerializeField] private CubeSlingData _cubeSlingData;
        [SerializeField] private CubeCombinerData _cubeCombinerData;
        [SerializeField] private AutoMergeBoosterData _autoMergeBoosterData;
        [SerializeField] private GameRulesData _gameRulesData;

        public override void InstallBindings()
        {
            ValidateReferences();

            Container.BindInstance(_cubeSpawnerData).IfNotBound();
            Container.BindInstance(_cubeSlingData).IfNotBound();
            Container.BindInstance(_cubeCombinerData).IfNotBound();
            Container.BindInstance(_autoMergeBoosterData).IfNotBound();
            Container.BindInstance(_gameRulesData).IfNotBound();
            
            Container.Bind<IDragInputFactory>().To<DragInputFactory>().AsSingle();
            Container.Bind<ICubeCombiner>().To<CubeCombiner>().AsSingle();
            Container.Bind<IScoreService>().To<ScoreService>().AsSingle();
            Container.Bind<IGameStateService>().To<GameStateService>().AsSingle();
            Container.Bind<IAutoMergeBooster>().To<AutoMergeBooster>().AsSingle();

            Container.Bind<ICubeSpawner>().To<CubeSpawner>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<CubeSling>().AsSingle();
            Container.BindInterfacesTo<GameFlow>().AsSingle();
        }

        private void ValidateReferences()
        {
            if (_cubeSpawnerData == null ||
                _cubeSlingData == null ||
                _cubeCombinerData == null ||
                _autoMergeBoosterData == null ||
                _gameRulesData == null)
            {
                throw new InvalidOperationException(
                    "ProjectInstaller has unassigned ScriptableObject references. Assign all data assets in the inspector.");
            }
        }
    }
}
