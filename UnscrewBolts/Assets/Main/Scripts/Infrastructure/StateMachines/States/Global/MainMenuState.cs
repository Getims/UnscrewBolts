using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Load;

namespace Scripts.Infrastructure.StateMachines.States.Global
{
    public class MainMenuState : IEnterState, IExitState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GlobalEventProvider _globalEventsProvider;
        private readonly IProgressDataService _progressDataService;

        public MainMenuState(IGameStateMachine stateMachine, GlobalEventProvider globalEventsProvider,
            IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _globalEventsProvider = globalEventsProvider;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _globalEventsProvider.AddListener<PlayClickedEvent>(OnPlayClick);
        }

        public void Exit()
        {
            _globalEventsProvider.RemoveListener<PlayClickedEvent>(OnPlayClick);
        }

        private void OnPlayClick()
        {
            _progressDataService.SetLevelStep(0);
            _stateMachine.Enter<LoadGameSceneState>();
        }
    }
}