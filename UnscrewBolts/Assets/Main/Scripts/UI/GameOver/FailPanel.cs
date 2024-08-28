using System;
using Scripts.GameLogic.Sound;
using Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.GameOver
{
    public class FailPanel : UIPanel
    {
        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _addTimeButton;

        private ISoundService _soundService;

        public event Action OnRestartClick;
        public event Action OnAddTimeClick;

        [Inject]
        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        private void Start()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _addTimeButton.onClick.AddListener(OnMenuButtonClick);
            _soundService.PlayFailSound();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick() =>
            OnRestartClick?.Invoke();

        private void OnMenuButtonClick() =>
            OnAddTimeClick?.Invoke();
    }
}