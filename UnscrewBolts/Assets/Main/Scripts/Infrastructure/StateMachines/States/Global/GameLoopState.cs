using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Load;

namespace Scripts.Infrastructure.StateMachines.States.Global
{
    public class GameLoopState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly GlobalEventProvider _globalEventsProvider;

        public GameLoopState(IGameStateMachine gameStateMachine, GlobalEventProvider globalEventsProvider)
        {
            _globalEventsProvider = globalEventsProvider;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _globalEventsProvider.AddListener<MainMenuButtonClickEvent>(OnMainMenuClick);
            _globalEventsProvider.AddListener<ReloadLevelEvent>(OnReload);
        }

        public void Exit()
        {
            _globalEventsProvider.RemoveListener<MainMenuButtonClickEvent>(OnMainMenuClick);
            _globalEventsProvider.RemoveListener<ReloadLevelEvent>(OnReload);
        }

        private void OnMainMenuClick() =>
            _gameStateMachine.Enter<LoadMainMenuSceneState>();

        private void OnReload() =>
            _gameStateMachine.Enter<LoadGameSceneState>();
    }
}