using System;
using Scripts.UI.Base;
using Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu
{
    public class TopMenuPanel : UIPanel
    {
        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private Button _shopButton;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        public event Action OnSettingsClick;
        public event Action OnShopClick;
        public Vector3 MoneyIconPosition => _moneyCounter.IconPosition;

        public override void Show()
        {
            base.Show();
            _moneyCounter.UpdateInfo();
        }

        private void Start()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _shopButton.onClick.AddListener(OnShopButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _shopButton.onClick.RemoveListener(OnShopButtonClick);
        }

        private void OnSettingsButtonClick() => OnSettingsClick?.Invoke();
        private void OnShopButtonClick() => OnShopClick?.Invoke();
    }
}