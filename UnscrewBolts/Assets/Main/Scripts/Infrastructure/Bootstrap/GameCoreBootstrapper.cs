using Scripts.Core.AppLogic;
using Scripts.Infrastructure.StateMachines;
using Scripts.Infrastructure.StateMachines.States.Global;
using Scripts.Infrastructure.StateMachines.States.Load;
using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Bootstrap
{
    public class GameCoreBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;
        private StateMachineFactory _stateMachineFactory;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, StateMachineFactory stateMachineFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateMachineFactory = stateMachineFactory;
        }

        private void Start()
        {
            SetupFramerate();
            SetupGameStates();
            EnterBootstrapState();
        }

        private void SetupGameStates()
        {
            _stateMachineFactory.BindState<LoadGameLoaderSceneState>(_gameStateMachine);
            _stateMachineFactory.BindState<GameLoaderState>(_gameStateMachine);

            _stateMachineFactory.BindState<LoadMainMenuSceneState>(_gameStateMachine);
            _stateMachineFactory.BindState<MainMenuState>(_gameStateMachine);

            _stateMachineFactory.BindState<LoadGameSceneState>(_gameStateMachine);
            _stateMachineFactory.BindState<GameLoopState>(_gameStateMachine);
        }

        private void SetupFramerate() =>
            FramerateSetter.SetDefaultFramerate();

        private void EnterBootstrapState() =>
            _gameStateMachine.Enter<LoadGameLoaderSceneState>();
    }
}