using UnityEngine;

namespace Test.Game2048.Boosters
{
    [CreateAssetMenu(menuName = "Game/Booster/AutoMerge")]
    public class AutoMergeBoosterData : ScriptableObject
    {
        public float RiseHeight => _riseHeight;
        public float RiseDuration => _riseDuration;
        public float SwingBackDistance => _swingBackDistance;
        public float SwingBackDuration => _swingBackDuration;
        public float MergeFlightDuration => _mergeFlightDuration;
        public ParticleSystem MergeParticles => _mergeParticles;

        [Header("Animation")]
        [SerializeField] private float _riseHeight = 2.5f;
        [SerializeField] private float _riseDuration = 0.2f;
        [SerializeField] private float _swingBackDistance = 0.35f;
        [SerializeField] private float _swingBackDuration = 0.15f;
        [SerializeField] private float _mergeFlightDuration = 0.25f;

        [Header("Optional VFX")]
        [SerializeField] private ParticleSystem _mergeParticles;
    }
}

