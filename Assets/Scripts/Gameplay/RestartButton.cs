using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Test.Game2048.Gameplay
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IGameRestartService _restartService;

        [Inject]
        private void Construct(IGameRestartService restartService)
        {
            _restartService = restartService;
        }

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_button != null)
                _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (_restartService == null || _button == null)
                return;

            _button.interactable = false;
            _restartService.Restart();
            _button.interactable = true;
        }
    }
}
