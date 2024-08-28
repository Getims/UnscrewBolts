using System;
using Scripts.Configs.Levels;
using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Ads;
using Scripts.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.GameMenu.Boosters
{
    public class BoosterWindow : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _useMoneyButton;

        [SerializeField]
        private Button _useAdsButton;

        [SerializeField]
        private Image _boosterIcon;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _boosterCost;

        private ISoundService _soundService;
        private IPlayerDataService _playerDataService;
        private BoosterConfig _boosterConfig;

        public event Action OnCloseButtonClick;
        public event Action<BoosterType> OnUseBooster;

        [Inject]
        public void Construct(ISoundService soundService, IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
            _soundService = soundService;
        }

        public void Initialize(BoosterConfig boosterConfig)
        {
            _boosterConfig = boosterConfig;
            _boosterIcon.sprite = _boosterConfig.BoosterIcon;
            _boosterCost.text = _boosterConfig.MoneyCost.ToString();
            _title.text = BoosterNameConverter.GetBoosterName(_boosterConfig.BoosterType);
        }

        public override void Show()
        {
            base.Show();
            _soundService.PlayOpenMenuSound();
        }

        protected virtual void Start()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
            _useAdsButton.onClick.AddListener(OnAdsClick);
            _useMoneyButton.onClick.AddListener(OnMoneyClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
            _useAdsButton.onClick.RemoveListener(OnAdsClick);
            _useMoneyButton.onClick.RemoveListener(OnMoneyClick);
        }

        private void OnMoneyClick()
        {
            if (_playerDataService.Money < _boosterConfig.MoneyCost)
                return;

            _playerDataService.SpendMoney(_boosterConfig.MoneyCost);
            UseBooster();
        }

        private void OnAdsClick() =>
            AdsManager.ShowRewarded(OnUnlockThemeByAds);

        private void UseBooster()
        {
            Hide();
            OnUseBooster?.Invoke(_boosterConfig.BoosterType);
        }

        private void OnUnlockThemeByAds(bool canUse)
        {
            if (canUse)
                UseBooster();
        }

        private void OnCloseClick()
        {
            Hide();
            OnCloseButtonClick?.Invoke();
        }
    }
}