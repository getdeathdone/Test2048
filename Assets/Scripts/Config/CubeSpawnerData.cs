using Test.Game2048.Config.Animation;
using Test.Game2048.Cubes;
using UnityEngine;

namespace Test.Game2048.Config
{
    [CreateAssetMenu(menuName = "Game/CubeSpawner")]
    public class CubeSpawnerData : ScriptableObject
    {
        public Cube Prefab => _prefab;
        public Animator<Transform> SpawnAnimation => _spawnAnimation;
        public CubeNumberGenerator NumberGenerator => _numberGenerator;
        public CubeColors Colors => _colors;

        [Header("References")]
        [SerializeField] private Cube _prefab;
        [SerializeField] private Animator<Transform> _spawnAnimation;
        [SerializeField] private CubeNumberGenerator _numberGenerator;
        [SerializeField] private CubeColors _colors;
    }
}
