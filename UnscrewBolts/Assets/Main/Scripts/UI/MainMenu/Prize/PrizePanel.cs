using System;
using Scripts.UI.Base;
using Scripts.UI.Common.UIAnimator.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Prize
{
    public class PrizePanel : UIPanel
    {
        [SerializeField]
        private Button _unlockButton;

        [SerializeField]
        private Button _collectButton;

        [SerializeField]
        private Transform _coinIcon;

        [SerializeField]
        private TextMeshProUGUI _rewardTMP;

        [SerializeField]
        private UIAnimator _showAnimator;

        [SerializeField]
        private UIAnimator _openAnimator;

        public event Action OnPrizeCollectClick;
        public Vector3 CoinIconPosition => _coinIcon.position;

        public void Initialize(int reward)
        {
            _rewardTMP.text = $"+{reward}";
        }

        public override void Show()
        {
            base.Show();
            _showAnimator.Play();
        }

        public override void Show(bool instant)
        {
            base.Show(instant);
            _showAnimator.Play();
        }

        public override void Show(float delay)
        {
            base.Show(delay);
            _showAnimator.Play();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unlockButton.onClick.RemoveListener(OnUnlockButtonClick);
            _collectButton.onClick.RemoveListener(OnCollectButtonClick);
        }

        private void Start()
        {
            _unlockButton.onClick.AddListener(OnUnlockButtonClick);
            _collectButton.onClick.AddListener(OnCollectButtonClick);
        }

        private void OnUnlockButtonClick()
        {
            _showAnimator.Stop();
            _openAnimator.Play();
        }

        private void OnCollectButtonClick() => OnPrizeCollectClick?.Invoke();
    }
}