using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Configs.Levels;
using Scripts.Data;
using Scripts.Data.Services;
using Scripts.Infrastructure.Actions;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.UI.Base;
using Scripts.UI.Common.FlyIcons;
using Scripts.UI.Common.Settings;
using Scripts.UI.Factories;
using Scripts.UI.MainMenu.Actions;
using Scripts.UI.MainMenu.Levels;
using Scripts.UI.MainMenu.Prize;
using Scripts.UI.MainMenu.Shop;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.UI.MainMenu
{
    public class MainMenuPanel : UIPanel
    {
        private readonly Queue<IAction> _showActions = new Queue<IAction>();

        private UIMenuFactory _menuFactory;
        private UIContainerProvider _uiContainerProvider;
        private IProgressDataService _progressDataService;
        private IPlayerDataService _playerDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;

        private FlyIconAnimationHandler _flyIconHandler;
        private SettingsPanel _settingsPanel;
        private TopMenuPanel _topMenuPanel;
        private LevelsPanel _levelsPanel;
        private ShopPanel _shopPanel;
        private LevelCompletePanel _levelCompletePanel;
        private PrizePanel _prizePanel;
        private Coroutine _showCO;

        public event Action OnPlayButtonClick;

        [Inject]
        public void Construct(UIContainerProvider uiContainerProvider, IProgressDataService progressDataService,
            IPlayerDataService playerDataService, IGameLevelsConfigProvider gameLevelsConfigProvider)
        {
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _playerDataService = playerDataService;
            _progressDataService = progressDataService;
            _uiContainerProvider = uiContainerProvider;
        }

        public void Initialize(UIMenuFactory menuFactory, FlyIconAnimationHandler flyIconAnimationHandler)
        {
            _menuFactory = menuFactory;
            _flyIconHandler = flyIconAnimationHandler;

            CreateTopPanel();
            CreateLevelsPanel();
            CreateShowActions();

            if (_showCO != null)
                StopCoroutine(_showCO);
            _showCO = StartCoroutine(PlayActionsQueue());
        }

        protected override void OnDestroy()
        {
            if (_showCO != null)
                StopCoroutine(_showCO);
        }

        [Button]
        private void CreateSettingsPanel()
        {
            if (_settingsPanel == null)
                _settingsPanel = _menuFactory.Create<SettingsPanel>(_uiContainerProvider.WindowsContainer);

            _settingsPanel.Show();
        }

        [Button]
        private void CreateTopPanel()
        {
            if (_topMenuPanel == null)
            {
                _topMenuPanel = _menuFactory.Create<TopMenuPanel>(_uiContainerProvider.MenuContainer);
                _topMenuPanel.OnSettingsClick += OnSettingsClick;
                _topMenuPanel.OnShopClick += OnShopClick;
            }

            _topMenuPanel.Show();
        }

        [Button]
        private void CreateLevelsPanel()
        {
            if (_levelsPanel == null)
            {
                _levelsPanel = _menuFactory.Create<LevelsPanel>(_uiContainerProvider.MenuContainer);
                _levelsPanel.OnPlayClick += OnPlayClick;
                _levelsPanel.Initialize();
            }

            _levelsPanel.Show();
        }

        [Button]
        private void CreateShopPanel()
        {
            if (_shopPanel == null)
            {
                _shopPanel = _menuFactory.Create<ShopPanel>(_uiContainerProvider.WindowsContainer);
                _shopPanel.Initialize();
            }

            _shopPanel.Show();
        }

        [Button]
        private void CreateLevelCompletePanel(bool show)
        {
            if (_levelCompletePanel == null)
                _levelCompletePanel = _menuFactory.Create<LevelCompletePanel>(_uiContainerProvider.WindowsContainer);

            if (show)
                _levelCompletePanel.Show();
        }

        [Button]
        private void CreatePrizePanel(bool show)
        {
            if (_prizePanel == null)
                _prizePanel = _menuFactory.Create<PrizePanel>(_uiContainerProvider.WindowsContainer);

            if (show)
                _prizePanel.Show();
        }

        private IEnumerator PlayActionsQueue()
        {
            while (_showActions.Count > 0)
            {
                IAction currentAction = _showActions.Dequeue();
                yield return StartCoroutine(currentAction.Execute());
            }
        }

        private void CreateShowActions()
        {
            _showActions.Enqueue(new WaitAction(0.05f));
            _showActions.Enqueue(new UpdateLevelsButtonsState(_levelsPanel, true));
            _showActions.Enqueue(new WaitAction(0.15f));

            if (!_progressDataService.HasReward)
                return;

            int reward = GetLevelReward();
            if (reward > 0)
                _showActions.Enqueue(new ShowRewardPanel(_levelsPanel, reward));

            CreateLevelCompletePanel(false);
            _showActions.Enqueue(new ShowLevelCompletePanel(() => _levelCompletePanel));

            int prizeReward = GetPrizeReward();
            if (_levelsPanel.HasPrize() && prizeReward != 0)
            {
                CreatePrizePanel(false);
                _showActions.Enqueue(new ShowPrizePanel(_prizePanel, prizeReward, 0.2f));
                _showActions.Enqueue(new ShowRewardAction(() => _prizePanel.CoinIconPosition,
                    () => _topMenuPanel.MoneyIconPosition, _flyIconHandler));
                _showActions.Enqueue(new AddMoneyAction(_playerDataService, prizeReward));
                _showActions.Enqueue(new HidePrizePanel(_prizePanel, 0.2f));
            }

            _showActions.Enqueue(new SwitchLevel(_progressDataService));
            _showActions.Enqueue(new UpdateLevelsButtonsState(_levelsPanel, false));

            if (reward != 0)
            {
                _showActions.Enqueue(new ShowRewardAction(() => _levelsPanel.CoinIconPosition,
                    () => _topMenuPanel.MoneyIconPosition, _flyIconHandler, 0.3f));
                _showActions.Enqueue(new AddMoneyAction(_playerDataService, reward));
                _showActions.Enqueue(new HideRewardPanel(_levelsPanel, 0.3f));
            }

            _showActions.Enqueue(new HideLevelCompletePanel(() => _levelCompletePanel, 0.75f));
        }

        private int GetLevelReward()
        {
            int currentLevel = _progressDataService.CurrentLevel;
            LevelData levelData = _progressDataService.GetLevelData(currentLevel);
            if (levelData.IsLevelComplete())
                return 0;

            LevelConfig levelConfig = _gameLevelsConfigProvider.GetLevel(currentLevel);
            return levelConfig.Reward;
        }

        private int GetPrizeReward()
        {
            int currentLevel = _progressDataService.CurrentLevel;
            LevelData levelData = _progressDataService.GetLevelData(currentLevel);
            if (levelData.IsLevelComplete())
                return 0;

            LevelConfig levelConfig = _gameLevelsConfigProvider.GetLevel(currentLevel);
            PrizeConfig prizeConfig = levelConfig.PrizeConfig;
            if (!prizeConfig.HasPrize)
                return 0;

            return prizeConfig.MoneyReward;
        }

        [Button]
        private void OnPlayClick()
        {
            Hide();
            OnPlayButtonClick?.Invoke();
        }

        private void OnSettingsClick() => CreateSettingsPanel();
        private void OnShopClick() => CreateShopPanel();
    }
}