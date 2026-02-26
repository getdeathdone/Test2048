using UnityEngine;

namespace Test.Game2048.Config
{
    [CreateAssetMenu(menuName = "Game/CubeSling")]
    public class CubeSlingData : ScriptableObject
    {
        public float PushForce => _pushForce;
        public float MinX => _minX;
        public float MaxX => _maxX;

        [Header("Parameters")]
        [SerializeField] private float _pushForce;
        [SerializeField] private float _minX = -2.25f;
        [SerializeField] private float _maxX = 2.25f;
    }
}
