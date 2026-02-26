using TMPro;
using UnityEngine;
using Zenject;

namespace Test.Game2048.GameState
{
    public class GameOverTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        private IGameStateService _gameState;

        [Inject]
        private void Construct(IGameStateService gameState)
        {
            _gameState = gameState;
        }

        private void Awake()
        {
            if (_label != null)
                _label.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_gameState == null)
                return;

            _gameState.GameOver += OnGameOver;
            _gameState.Changed += OnStateChanged;
        }

        private void OnDisable()
        {
            if (_gameState == null)
                return;

            _gameState.GameOver -= OnGameOver;
            _gameState.Changed -= OnStateChanged;
        }

        private void OnGameOver()
        {
            if (_label != null)
            {
                _label.text = "Game Over";
                _label.gameObject.SetActive(true);
            }
        }

        private void OnStateChanged(bool isGameOver)
        {
            if (_label == null)
                return;

            _label.gameObject.SetActive(isGameOver);
        }
    }
}

