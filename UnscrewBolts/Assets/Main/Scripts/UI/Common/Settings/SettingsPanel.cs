using System;
using Scripts.GameLogic.Sound;
using Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.Common.Settings
{
    public class SettingsPanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private SoundButton _soundButton;

        [SerializeField]
        private SoundButton _musicButton;

        private ISoundService _soundService;

        public event Action OnCloseButtonClick;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public override void Show()
        {
            base.Show();
            UpdateInfo();
            _soundService.PlayOpenMenuSound();
        }

        protected virtual void Start() =>
            _closeButton.onClick.AddListener(OnCloseClick);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
        }

        private void UpdateInfo()
        {
            _soundButton.UpdateInfo();
            _musicButton.UpdateInfo();
        }

        private void OnCloseClick()
        {
            Hide();
            OnCloseButtonClick?.Invoke();
        }
    }
}