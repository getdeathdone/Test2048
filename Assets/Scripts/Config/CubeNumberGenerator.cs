using UnityEngine;

namespace Test.Game2048.Config
{
    [CreateAssetMenu(menuName = "Game/CubeNumberGenerator")]
    public class CubeNumberGenerator : ScriptableObject
    {
        [SerializeField] private int _base = 2;
        [SerializeField, Range(0f, 1f)] private float _spawnTwoChance = 0.75f;

        public int Generate()
        {
            return Random.value <= _spawnTwoChance ? 2 : 4;
        }
        public int GenerateNext(int number)
        {
            var power = GetNextPower(number);
            return GetNumber(power);
        }

        public int GetIndex(int number)
        {
            var power = GetPower(number);
            var index = power - 1;

            return index;
        }
        public int GetNextPower(int number)
        {
            return GetPower(number) + 1;
        }
        public int GetPower(int number)
        {
            return (int)Mathf.Log(number, _base);
        }
        public int GetNumber(int power)
        {
            return (int)Mathf.Pow(_base, power);
        }
    }
}

