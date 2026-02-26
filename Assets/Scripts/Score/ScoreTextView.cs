using TMPro;
using UnityEngine;
using Zenject;

namespace Test.Game2048.Score
{
    public class ScoreTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private IScoreService _scoreService;

        [Inject]
        private void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void OnEnable()
        {
            if (_scoreService == null)
                return;

            _scoreService.Changed += OnScoreChanged;
            OnScoreChanged(_scoreService.Current);
        }

        private void OnDisable()
        {
            if (_scoreService == null)
                return;

            _scoreService.Changed -= OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {score}";
        }
    }
}

