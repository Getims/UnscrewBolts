using Scripts.Configs.Levels;
using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.UI.Common.Settings;
using Scripts.UI.Factories;
using Scripts.UI.GameMenu;
using Scripts.UI.GameMenu.Boosters;
using Scripts.UI.Tutorial;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class GamePlayState : IEnterState, IExitState
    {
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly LocalEventProvider _localEventProvider;
        private readonly GameStateMachine _stateMachine;
        private readonly IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private readonly IProgressDataService _progressDataService;

        private IAnchorStateSetter _anchorStateSetter;
        private TopGamePanel _topGamePanel;
        private BoostersPanel _boostersPanel;
        private PausePanel _pausePanel;
        private BoosterWindow _boosterWindow;
        private TutorialPanel _tutorialPanel;

        public GamePlayState(GameStateMachine stateMachine, LocalEventProvider localEventProvider,
            IUIMenuFactory uiMenuFactory, IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _stateMachine = stateMachine;
            _localEventProvider = localEventProvider;
            _uiMenuFactory = uiMenuFactory;
        }

        public void Enter()
        {
            if (_topGamePanel == null)
            {
                _topGamePanel = _uiMenuFactory.GetPanel<TopGamePanel>();
                _topGamePanel.OnSettingsClick += OnPauseButtonClick;
            }

            if (_boostersPanel == null)
            {
                _boostersPanel = _uiMenuFactory.GetPanel<BoostersPanel>();
                _boostersPanel.OnBoosterSelect += OnBoosterSelect;
            }

            if (_progressDataService.CurrentLevel == 0 && !_progressDataService.IsTutorialComplete)
            {
                _tutorialPanel = _uiMenuFactory.GetPanel<TutorialPanel>();
                _tutorialPanel.OnTutorialCompleteEvent += OnTutorialComplete;
            }

            _localEventProvider.AddListener<GameOverEvent, bool>(OnGameOver);
            _localEventProvider.AddListener<TryToUnlockAnchorEvent, IAnchorStateSetter>(OnTryToUnlockAnchor);
        }

        public void Exit()
        {
            _localEventProvider.RemoveListener<GameOverEvent, bool>(OnGameOver);
            _localEventProvider.RemoveListener<TryToUnlockAnchorEvent, IAnchorStateSetter>(OnTryToUnlockAnchor);

            if (_pausePanel != null)
                _pausePanel.Hide();
            if (_boosterWindow != null)
                _boosterWindow.Hide();
        }

        private void CreatePausePanel()
        {
            if (_pausePanel == null)
            {
                _pausePanel = _uiMenuFactory.Create<PausePanel>();
                _pausePanel.OnResetButtonClick += OnResetButtonClick;
                _pausePanel.OnMainMenuButtonClick += OnMainMenuButtonClick;
            }

            _pausePanel.Show();
        }

        private void CreateBoosterWindow(BoosterConfig boosterConfig)
        {
            if (_boosterWindow == null)
            {
                _boosterWindow = _uiMenuFactory.Create<BoosterWindow>();
                _boosterWindow.OnUseBooster += OnUseBooster;
            }

            _boosterWindow.Initialize(boosterConfig);
            _boosterWindow.Show();
        }

        private void OnGameOver(bool isWin) =>
            _stateMachine.Enter<GameOverState, bool>(isWin);

        private void OnPauseButtonClick() => CreatePausePanel();

        private void OnBoosterSelect(BoosterConfig boosterConfig) => CreateBoosterWindow(boosterConfig);

        private void OnResetButtonClick() =>
            _stateMachine.Enter<ResetGameState, GameResetType>(GameResetType.Full);

        private void OnMainMenuButtonClick() =>
            _stateMachine.Enter<ExitGameState>();

        private void OnTryToUnlockAnchor(IAnchorStateSetter anchorStateSetter)
        {
            _anchorStateSetter = anchorStateSetter;
            BoosterConfig unlockAnchorBooster = _gameLevelsConfigProvider.Config.UnlockAnchorBoosterConfig;
            CreateBoosterWindow(unlockAnchorBooster);
        }

        private void OnUseBooster(BoosterType boosterType)
        {
            switch (boosterType)
            {
                case BoosterType.Restart:
                    _stateMachine.Enter<ResetGameState, GameResetType>(GameResetType.CurrentStep);
                    break;
                case BoosterType.UnlockAnchor:
                    if (_anchorStateSetter != null)
                        _anchorStateSetter.Unlock();
                    break;
                case BoosterType.UnscrewBolt:
                    _stateMachine.Enter<UnscrewBoosterState>();
                    break;
                case BoosterType.Bomb:
                    _stateMachine.Enter<BombBoosterState>();
                    break;
                default:
                    _stateMachine.Enter<GamePlayState>();
                    break;
            }
        }

        private void OnTutorialComplete()
        {
            _tutorialPanel.OnTutorialCompleteEvent -= OnTutorialComplete;
            _progressDataService.SetTutorialState(true);
        }
    }
}