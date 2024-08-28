using System;
using Scripts.Configs.Levels;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.UI.Base;
using Scripts.UI.Factories;
using UnityEngine;
using Zenject;

namespace Scripts.UI.MainMenu.Levels
{
    public class LevelsPanel : UIPanel
    {
        [SerializeField]
        private PlayButton _playButton;

        [SerializeField]
        private RewardPanel _rewardPanel;

        [SerializeField]
        private ButtonsCreator _buttonsCreator = new ButtonsCreator();

        [SerializeField]
        private LevelsScroller _levelsScroller = new LevelsScroller();

        private IProgressDataService _progressDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;

        public event Action OnPlayClick;
        public Vector3 CoinIconPosition => _rewardPanel.CoinIconPosition;

        [Inject]
        public void Construct(IUIElementFactory uiElementFactory, IProgressDataService progressDataService,
            IGameLevelsConfigProvider gameLevelsConfigProvider)
        {
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _progressDataService = progressDataService;
            _buttonsCreator.Initialize(uiElementFactory, gameLevelsConfigProvider, progressDataService);
        }

        public void Initialize()
        {
            _levelsScroller.SetState(false);

            _buttonsCreator.CreateButtons(OnLevelClick);
            UpdateButtonsState();

            _levelsScroller.Initialize();
            _levelsScroller.SetState(true);
            _levelsScroller.ScrollToLevel(_progressDataService.CurrentLevel);
        }

        public float UpdateInfo(bool instant = false)
        {
            int currentLevel = _progressDataService.CurrentLevel;
            _levelsScroller.ScrollToLevel(currentLevel, instant);
            UpdateButtonsState();
            return _levelsScroller.ScrollTime;
        }

        public bool HasPrize()
        {
            int currentLevel = _progressDataService.CurrentLevel;
            LevelConfig levelConfig = _gameLevelsConfigProvider.GetLevel(currentLevel);
            return levelConfig.PrizeConfig.HasPrize;
        }

        public void ShowRewardPanel(int reward)
        {
            _rewardPanel.SetValue(reward);
            _rewardPanel.Show();
            _playButton.Hide();
        }

        public void HideRewardPanel()
        {
            _rewardPanel.Hide();
            _playButton.Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _playButton.OnPlayClick -= OnPlayButtonClick;
            _levelsScroller.OnDestroy();
        }

        private void Start()
        {
            _playButton.OnPlayClick += OnPlayButtonClick;
        }

        private void OnLevelClick(int levelIndex)
        {
            _levelsScroller.ScrollToLevel(levelIndex);
            _progressDataService.SetCurrentLevel(levelIndex);
            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            LevelButton levelButton;
            int currentLevelIndex = _progressDataService.CurrentLevel;

            for (int i = 0; i < _buttonsCreator.ButtonsCount; i++)
            {
                levelButton = _buttonsCreator.CreatedButtons[i];
                levelButton.UpdateState(currentLevelIndex);
            }
        }

        private void OnPlayButtonClick() => OnPlayClick?.Invoke();
    }
}