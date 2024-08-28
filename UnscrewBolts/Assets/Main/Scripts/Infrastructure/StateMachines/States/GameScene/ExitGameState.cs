using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class ExitGameState : IEnterState
    {
        private readonly GlobalEventProvider _globalEventsProvider;

        public ExitGameState(GlobalEventProvider globalEventsProvider) =>
            _globalEventsProvider = globalEventsProvider;

        public void Enter() =>
            _globalEventsProvider.Invoke<MainMenuButtonClickEvent>();
    }
}