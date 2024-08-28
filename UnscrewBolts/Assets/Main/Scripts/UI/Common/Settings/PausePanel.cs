using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Common.Settings
{
    public class PausePanel : SettingsPanel
    {
        [SerializeField]
        private Button _mainMenuButton;

        [SerializeField]
        private Button _resetButton;

        public event Action OnResetButtonClick;
        public event Action OnMainMenuButtonClick;

        protected override void Start()
        {
            base.Start();
            _mainMenuButton.onClick.AddListener(OnMainMenuClick);
            _resetButton.onClick.AddListener(OnResetClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _mainMenuButton.onClick.RemoveListener(OnMainMenuClick);
            _resetButton.onClick.RemoveListener(OnResetClick);
        }

        private void OnMainMenuClick()
        {
            Hide();
            OnMainMenuButtonClick?.Invoke();
        }

        private void OnResetClick()
        {
            Hide();
            OnResetButtonClick?.Invoke();
        }
    }
}