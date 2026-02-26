using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Test.Game2048.Boosters
{
    public class AutoMergeBoosterButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IAutoMergeBooster _booster;

        [Inject]
        private void Construct(IAutoMergeBooster booster)
        {
            _booster = booster;
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
            HandleClickAsync().Forget();
        }

        private async UniTaskVoid HandleClickAsync()
        {
            if (_button == null || _booster == null || _booster.IsBusy)
                return;

            _button.interactable = false;
            try
            {
                await _booster.TryExecuteAsync();
            }
            finally
            {
                _button.interactable = true;
            }
        }
    }
}

