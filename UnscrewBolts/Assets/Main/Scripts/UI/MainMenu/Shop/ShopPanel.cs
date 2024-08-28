using System.Collections.Generic;
using System.Collections.ObjectModel;
using Scripts.Configs.Player;
using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Ads;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI.Base;
using Scripts.UI.Factories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.MainMenu.Shop
{
    public class ShopPanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private SelectButton _selectButton;

        [SerializeField]
        private Transform _themesContainer;

        [SerializeField]
        private ThemeButton _themeButtonPrefab;

        [SerializeField]
        private List<ThemeButton> _themesButtons;

        private IPlayerConfigProvider _playerConfigProvider;
        private IThemeDataService _themeDataService;
        private LocalEventProvider _localEventProvider;
        private IPlayerDataService _playerDataService;
        private string _selectedThemeId;
        private IUIElementFactory _uiElementFactory;
        private ISoundService _soundService;

        [Inject]
        public void Construct(IPlayerConfigProvider playerConfigProvider, IPlayerDataService playerDataService,
            IThemeDataService themeDataService, IUIElementFactory uiElementFactory,
            LocalEventProvider localEventProvider, ISoundService soundService)
        {
            _soundService = soundService;
            _uiElementFactory = uiElementFactory;
            _playerDataService = playerDataService;
            _localEventProvider = localEventProvider;
            _themeDataService = themeDataService;
            _playerConfigProvider = playerConfigProvider;
        }

        public void Initialize()
        {
            CreateThemeButtons();
            _selectedThemeId = _themeDataService.CurrentThemeID;
            UpdateButtonsState();
            UpdateSelectButton();
        }

        public override void Show()
        {
            base.Show();
            _soundService.PlayOpenMenuSound();
        }

        protected virtual void Start()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
            _selectButton.OnClick += OnSelectClick;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
            _selectButton.OnClick -= OnSelectClick;
        }

        private void CreateThemeButtons()
        {
            ReadOnlyCollection<ThemeConfig> themesConfigs = _playerConfigProvider.Config.Themes;

            foreach (ThemeConfig themeConfig in themesConfigs)
            {
                var themeButton = _uiElementFactory.Create(_themeButtonPrefab, _themesContainer);
                themeButton.Initialize(themeConfig);
                themeButton.OnClick += OnSelectTheme;
                _themesButtons.Add(themeButton);
            }
        }

        private void UpdateButtonsState()
        {
            string currentThemeId = _themeDataService.CurrentThemeID;

            foreach (ThemeButton themeButton in _themesButtons)
            {
                bool isUnlocked = _themeDataService.IsThemeUnlocked(themeButton.ThemeId);
                themeButton.UpdateState(currentThemeId, _selectedThemeId, isUnlocked);
            }
        }

        private void UpdateSelectButton()
        {
            ThemeConfig themeConfig = _playerConfigProvider.Config.GetTheme(_selectedThemeId);
            _selectButton.Initialize(themeConfig, _themeDataService.IsThemeUnlocked(_selectedThemeId));
        }

        private void UnlockTheme()
        {
            _themeDataService.UnlockTheme(_selectedThemeId);
            _themeDataService.SetCurrentTheme(_selectedThemeId);
            _localEventProvider.Invoke<ThemeSwitchEvent>();

            UpdateButtonsState();
            UpdateSelectButton();
        }

        private void OnCloseClick() => Hide();

        private void OnSelectTheme(ThemeConfig themeConfig)
        {
            _selectedThemeId = themeConfig.ThemeId;

            UpdateButtonsState();
            UpdateSelectButton();
        }

        private void OnSelectClick()
        {
            if (_themeDataService.IsThemeUnlocked(_selectedThemeId))
            {
                _themeDataService.SetCurrentTheme(_selectedThemeId);
                _localEventProvider.Invoke<ThemeSwitchEvent>();

                UpdateButtonsState();
                UpdateSelectButton();
                return;
            }

            ThemeConfig themeConfig = _playerConfigProvider.Config.GetTheme(_selectedThemeId);
            switch (themeConfig.UnlockType)
            {
                case CurrencyType.Free:
                    UnlockTheme();
                    break;
                case CurrencyType.AdsWatch:
                    AdsManager.ShowRewarded(OnUnlockThemeByAds);
                    break;
                case CurrencyType.Money:
                    if (_playerDataService.Money < themeConfig.MoneyAmount)
                        return;
                    _playerDataService.SpendMoney(themeConfig.MoneyAmount);
                    UnlockTheme();
                    break;
            }
        }

        private void OnUnlockThemeByAds(bool unlock)
        {
            if (!unlock)
                return;

            UnlockTheme();
        }
    }
}