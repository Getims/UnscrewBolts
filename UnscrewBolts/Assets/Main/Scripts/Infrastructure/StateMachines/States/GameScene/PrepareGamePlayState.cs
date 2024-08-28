using System.Collections;
using Scripts.Core.Constants;
using Scripts.Data.Services;
using Scripts.GameLogic.GameFlow;
using Scripts.Infrastructure.Bootstrap;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.UI;
using Scripts.UI.Common.FlyIcons;
using Scripts.UI.Factories;
using Scripts.UI.GameMenu;
using Scripts.UI.GameMenu.Boosters;
using Scripts.UI.Tutorial;
using UnityEngine;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class PrepareGamePlayState : IEnterState, IExitState
    {
        private const float SCENE_LOAD_TIME = 0.15f;

        private readonly GameStateMachine _stateMachine;
        private readonly Transform _coinsContainer;
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameFlowProvider _gameFlowProvider;
        private readonly IProgressDataService _progressDataService;

        private TopGamePanel _topGamePanel;
        private BoostersPanel _boostersPanel;
        private TutorialPanel _tutorialPanel;
        private Coroutine _levelCreationCO;

        public PrepareGamePlayState(GameStateMachine stateMachine, UIContainerProvider uiContainerProvider,
            IUIMenuFactory uiMenuFactory, ICoroutineRunner coroutineRunner, IGameFlowProvider gameFlowProvider,
            IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _gameFlowProvider = gameFlowProvider;
            _coroutineRunner = coroutineRunner;
            _uiMenuFactory = uiMenuFactory;
            _coinsContainer = uiContainerProvider.CoinsContainer;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            if (_levelCreationCO != null)
                _coroutineRunner.StopCoroutine(_levelCreationCO);

            _levelCreationCO = _coroutineRunner.StartCoroutine(CreateLevel());
        }

        public void Exit()
        {
            if (_levelCreationCO != null)
                _coroutineRunner?.StopCoroutine(_levelCreationCO);
        }

        private void CreateTopPanel(bool show = true)
        {
            if (_topGamePanel == null)
            {
                _topGamePanel = _uiMenuFactory.Create<TopGamePanel>();
                _topGamePanel.Initialize(CreateIconsFlyHandler());
            }

            if (show)
                _topGamePanel.Show();
        }

        private void CreateBoostersPanel(bool show = true)
        {
            if (_boostersPanel == null)
            {
                _boostersPanel = _uiMenuFactory.Create<BoostersPanel>();
                _boostersPanel.Initialize();
            }

            if (show)
                _boostersPanel.Show();
        }

        private FlyIconAnimationHandler CreateIconsFlyHandler()
        {
            FlyIconAnimationHandler flyIconAnimationHandler =
                _uiMenuFactory.Create<FlyIconAnimationHandler>(_coinsContainer);
            return flyIconAnimationHandler;
        }

        private void CreateTutorialPanel()
        {
            if (_tutorialPanel == null)
                _tutorialPanel = _uiMenuFactory.Create<TutorialPanel>();

            _tutorialPanel.Show();
        }

        IEnumerator CreateLevel()
        {
            yield return new WaitForSeconds(SCENE_LOAD_TIME);
            while (!_gameFlowProvider.IsLoaded)
                yield return new WaitForEndOfFrame();

            _gameFlowProvider.Initialize();
            yield return new WaitForEndOfFrame();

            if (_progressDataService.IsTutorialComplete)
            {
                CreateTopPanel();
                CreateBoostersPanel();
                yield return new WaitForEndOfFrame();

                yield return new WaitForSeconds(_topGamePanel.FadeTime);
                _gameFlowProvider.ShowLevel();
                yield return new WaitForSeconds(GameLogicConstants.LEVEL_FADE_TIME);

                _gameFlowProvider.StartTimer();
                _stateMachine.Enter<GamePlayState>();
            }
            else
            {
                CreateTopPanel(false);
                CreateBoostersPanel(false);
                CreateTutorialPanel();
                yield return new WaitForEndOfFrame();

                _gameFlowProvider.ShowLevel();
                yield return new WaitForSeconds(GameLogicConstants.LEVEL_FADE_TIME);

                _stateMachine.Enter<GamePlayState>();
            }
        }
    }
}