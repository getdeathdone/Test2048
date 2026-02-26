using Test.Game2048.Config;
using Test.Game2048.Cubes.Abstractions;
using UnityEngine;
using Zenject;

namespace Test.Game2048.Cubes
{
    public class CubeSpawner : MonoBehaviour, ICubeSpawner
    {
        [Header("References")]
        [SerializeField] private Transform _spawnPoint;
        
        private CubeSpawnerData _data;

        [Inject]
        private void Construct(CubeSpawnerData data)
        {
            _data = data;
        }

        public Cube SpawnRandom()
        {
            var randomNumber = _data.NumberGenerator.Generate();
            return Spawn(randomNumber);
        }

        private Cube Spawn(int number)
        {
            var index = _data.NumberGenerator.GetIndex(number);
            var color = _data.Colors.GetColor(index);

            return Spawn(_spawnPoint.position, number, color);
        }

        private Cube Spawn(Vector3 position, int number, Color color)
        {
            var cube = Instantiate(_data.Prefab, position, Quaternion.identity);
            cube.Initialize(number, color);

            _data.SpawnAnimation?.Animate(cube.transform);

            return cube;
        }
    }
}
