using UnityEngine;

namespace Test.Game2048.GameState
{
    [CreateAssetMenu(menuName = "Game/Rules")]
    public class GameRulesData : ScriptableObject
    {
        public int MaxCubesOnBoard => _maxCubesOnBoard;

        [SerializeField] private int _maxCubesOnBoard = 40;
    }
}

