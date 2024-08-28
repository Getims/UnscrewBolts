using Scripts.GameLogic.Sound;
using Scripts.UI.Base;
using Scripts.UI.Common.UIAnimator.Main;
using UnityEngine;
using Zenject;

namespace Scripts.UI.GameOver
{
    public class HardLevelPanel : UIPanel
    {
        [SerializeField]
        private UIAnimator _showAnimator;

        [SerializeField]
        private UIAnimator _hideAnimator;

        private ISoundService _soundService;

        public float ShowTime => _showAnimator.GetAnimatorWorkTime();
        public float HideTime => _hideAnimator.GetAnimatorWorkTime();

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public override void Show()
        {
            base.Show();
            _hideAnimator.Stop();
            _showAnimator.Play();
            _soundService.PlayHardModeSound();
        }

        public override void Hide()
        {
            base.Hide();
            _showAnimator.Stop();
            _hideAnimator.Play();
        }
    }
}