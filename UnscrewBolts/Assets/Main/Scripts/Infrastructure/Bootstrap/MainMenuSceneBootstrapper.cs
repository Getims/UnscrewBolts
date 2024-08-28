using ModestTree;
using Scripts.Configs.Player;
using Scripts.Data;
using Scripts.Data.Services;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI;
using Scripts.UI.Common.FlyIcons;
using Scripts.UI.Factories;
using Scripts.UI.MainMenu;
using Zenject;

namespace Scripts.Infrastructure.Bootstrap
{
    public class MainMenuSceneBootstrapper
    {
        private readonly ISoundService _soundService;
        private readonly IThemeDataService _themeDataService;
        private readonly IPlayerConfigProvider _playerConfigProvider;
        private readonly IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private readonly IProgressDataService _progressDataService;
        private readonly GlobalEventProvider _globalEventsProvider;
        private readonly UIContainerProvider _uiContainerProvider;

        private UIMenuFactory _uiMenuFactory;
        private MainMenuPanel _mainMenuPanel;

        public MainMenuSceneBootstrapper(DiContainer container, ISoundService soundService,
            IUIConfigProvider uiConfigProvider, GlobalEventProvider globalEventsProvider,
            UIContainerProvider uiContainerProvider, IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService, IThemeDataService themeDataService,
            IPlayerConfigProvider playerConfigProvider)
        {
            _uiContainerProvider = uiContainerProvider;
            _globalEventsProvider = globalEventsProvider;
            _progressDataService = progressDataService;
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _playerConfigProvider = playerConfigProvider;
            _themeDataService = themeDataService;
            _soundService = soundService;

            SetupMenuFactory(container, uiConfigProvider, uiContainerProvider);
        }

        public void Initialize()
        {
            SetupLevelsData();
            SetupThemeData();

            CreateMainMenuPanel();

            _mainMenuPanel.OnPlayButtonClick += OnPlayClick;
            _soundService.PlayMenuBackgroundMusic();
        }

        private void SetupMenuFactory(DiContainer container, IUIConfigProvider uiConfigProvider,
            UIContainerProvider uiContainerProvider)
        {
            _uiMenuFactory = new UIMenuFactory(container, uiConfigProvider.Config.MainMenuListConfig,
                uiContainerProvider.MenuContainer);

            container.Bind<IUIMenuFactory>().FromInstance(_uiMenuFactory).AsSingle().NonLazy();
        }

        private void SetupThemeData()
        {
            if (_themeDataService.UnlockedThemesID.Count != 0 && !_themeDataService.CurrentThemeID.IsEmpty())
                return;

            ThemeConfig baseTheme = _playerConfigProvider.Config.Themes[0];
            _themeDataService.UnlockTheme(baseTheme.ThemeId);
            _themeDataService.SetCurrentTheme(baseTheme.ThemeId);
        }

        private void SetupLevelsData()
        {
            int levelsCount = _gameLevelsConfigProvider.LevelsCount;
            int levelsInData = _progressDataService.LevelsCount;

            if (levelsInData == 0)
                _progressDataService.SetLevelState(0, true, false, false);
            else
            {
                LevelData levelData = _progressDataService.GetLevelData(0);
                _progressDataService.SetLevelState(0, true, levelData.IsLevelComplete(), false);
            }

            if (levelsInData >= levelsCount)
                return;

            int needLevels = levelsCount - levelsInData;
            for (int i = 0; i < needLevels; i++)
                _progressDataService.AddLevelData(false);

            _progressDataService.SaveData();
        }

        private void CreateMainMenuPanel()
        {
            if (_mainMenuPanel == null)
            {
                _mainMenuPanel = _uiMenuFactory.Create<MainMenuPanel>();
                _mainMenuPanel.Initialize(_uiMenuFactory, CreateIconsFlyHandler());
            }

            _mainMenuPanel.Show(true);
        }

        private FlyIconAnimationHandler CreateIconsFlyHandler()
        {
            FlyIconAnimationHandler flyIconAnimationHandler =
                _uiMenuFactory.Create<FlyIconAnimationHandler>(_uiContainerProvider.CoinsContainer);
            return flyIconAnimationHandler;
        }

        private void OnPlayClick()
        {
            _mainMenuPanel.OnPlayButtonClick -= OnPlayClick;
            _globalEventsProvider.Invoke<PlayClickedEvent>();
        }
    }
}