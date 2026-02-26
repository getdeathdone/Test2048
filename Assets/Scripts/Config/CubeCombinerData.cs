using UnityEngine;

namespace Test.Game2048.Config
{
    [CreateAssetMenu(menuName = "Game/CubeCombiner")]
    public class CubeCombinerData : ScriptableObject
    {
        public CubeNumberGenerator NumberGenerator => _numberGenerator;
        public CubeColors Colors => _colors;
        public float PushUpForce => _pushUpForce;
        public float CombineDuration => _combineDuration;
        public float MinDirectedSpeed => _minDirectedSpeed;

        [Header("References")]
        [SerializeField] private CubeNumberGenerator _numberGenerator;
        [SerializeField] private CubeColors _colors;
        [Header("Parameters")]
        [SerializeField] private float _pushUpForce;
        [SerializeField] private float _combineDuration;
        [SerializeField] private float _minDirectedSpeed = 1.5f;
    }
}

