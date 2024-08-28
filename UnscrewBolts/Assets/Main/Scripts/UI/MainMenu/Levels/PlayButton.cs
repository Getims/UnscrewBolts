using System;
using Scripts.UI.Base;
using Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Levels
{
    public class PlayButton : UIPanel
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private LevelTracker _levelTracker;

        public event Action OnPlayClick;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
        }

        public override void Show()
        {
            base.Show();
            _levelTracker.UpdateInfo();
        }

        public override void Show(bool instant)
        {
            base.Show(instant);
            _levelTracker.UpdateInfo();
        }

        private void OnPlayButtonClick() => OnPlayClick?.Invoke();
    }
}