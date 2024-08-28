using System.Collections;
using Scripts.Core.Constants;
using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.GameLogic.GameFlow;
using Scripts.Infrastructure.Ads;
using Scripts.Infrastructure.Bootstrap;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.UI;
using Scripts.UI.Factories;
using Scripts.UI.GameMenu;
using Scripts.UI.GameMenu.Boosters;
using Scripts.UI.GameOver;
using UnityEngine;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class GameOverState : IEnterState<bool>, IExitState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly UIContainerProvider _uiContainerProvider;
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly IGameFlowProvider _gameFlowProvider;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IProgressDataService _progressDataService;
        private readonly int _additionalTime;

        private FailPanel _failPanel;
        private LevelCompletePanel _levelCompletePanel;
        private HardLevelPanel _hardLevelPanel;
        private Coroutine _stepSwitchCO;

        public GameOverState(GameStateMachine stateMachine, UIContainerProvider uiContainerProvider,
            IGlobalConfigProvider globalConfigProvider, ICoroutineRunner coroutineRunner,
            IGameFlowProvider gameFlowProvider, IUIMenuFactory uiMenuFactory, IProgressDataService progressDataService)
        {
            _gameFlowProvider = gameFlowProvider;
            _uiMenuFactory = uiMenuFactory;
            _stateMachine = stateMachine;
            _uiContainerProvider = uiContainerProvider;
            _progressDataService = progressDataService;
            _coroutineRunner = coroutineRunner;
            _additionalTime = globalConfigProvider.Config.AdditionalTime;
        }

        public void Enter(bool isWin)
        {
            if (isWin)
                SwitchLevel();
            else
                CreateFailPanel();
        }

        public void Exit()
        {
        }

        private void SwitchLevel()
        {
            int currentStep = _progressDataService.CurrentLevelStep;
            if (_stepSwitchCO != null)
                _coroutineRunner.StopCoroutine(_stepSwitchCO);

            CreateLevelCompletePanel();
            if (currentStep == 0)
                _stepSwitchCO = _coroutineRunner.StartCoroutine(SwitchStep());
            else
                _stepSwitchCO = _coroutineRunner.StartCoroutine(CompleteLevel());
        }

        private void CreateFailPanel()
        {
            if (_failPanel == null)
            {
                _failPanel = _uiMenuFactory.Create<FailPanel>(_uiContainerProvider.WindowsContainer);
                _failPanel.OnRestartClick += OnRestartClick;
                _failPanel.OnAddTimeClick += OnAddTimeClick;
            }

            _failPanel.Show();
        }

        private void CreateLevelCompletePanel()
        {
            if (_levelCompletePanel == null)
                _levelCompletePanel = _uiMenuFactory.Create<LevelCompletePanel>();
        }

        private void CreateHardLevelPanel()
        {
            if (_hardLevelPanel == null)
                _hardLevelPanel = _uiMenuFactory.Create<HardLevelPanel>();
        }

        IEnumerator SwitchStep()
        {
            CreateHardLevelPanel();
            WaitForSeconds pause = new WaitForSeconds(0.15f);
            _progressDataService.SetLevelStep(1);
            yield return ShowLevelComplete(pause);

            _stateMachine.Enter<PrepareGamePlayState>();
            _hardLevelPanel.Show();

            yield return new WaitForSeconds(_hardLevelPanel.ShowTime);
            yield return pause;

            _hardLevelPanel.Hide();
        }

        IEnumerator CompleteLevel()
        {
            WaitForSeconds pause = new WaitForSeconds(0.15f);
            _progressDataService.SetRewardState(true);

            yield return ShowLevelComplete(pause);
            _stateMachine.Enter<ExitGameState>();
        }

        private IEnumerator ShowLevelComplete(WaitForSeconds pause)
        {
            TopGamePanel topGamePanel = _uiMenuFactory.GetPanel<TopGamePanel>();
            BoostersPanel boostersPanel = _uiMenuFactory.GetPanel<BoostersPanel>();
            yield return pause;

            topGamePanel.Hide();
            boostersPanel.Hide();
            yield return new WaitForSeconds(topGamePanel.FadeTime);

            _levelCompletePanel.Show();
            yield return new WaitForSeconds(_levelCompletePanel.ShowTime);
            yield return pause;

            _gameFlowProvider.HideLevel();
            yield return new WaitForSeconds(GameLogicConstants.LEVEL_FADE_TIME);
            yield return pause;

            _levelCompletePanel.Hide();
            yield return new WaitForSeconds(_levelCompletePanel.HideTime);
        }

        private void OnRestartClick() =>
            _stateMachine.Enter<ResetGameState, GameResetType>(GameResetType.Full);

        private void OnAddTimeClick() =>
            AdsManager.ShowRewarded(OnAddTimeByAds);

        private void OnAddTimeByAds(bool canAdd)
        {
            if (!canAdd)
                return;

            _failPanel.Hide();
            _gameFlowProvider.AddTimeToTimer(_additionalTime);
            _stateMachine.Enter<GamePlayState>();
        }
    }
}