using Scripts.Infrastructure.StateMachines.BaseStates;
using Zenject;

namespace Scripts.Infrastructure.StateMachines
{
    public class StateMachineFactory
    {
        private readonly DiContainer _diContainer;

        public StateMachineFactory(DiContainer diContainer) =>
            _diContainer = diContainer;

        public void BindState<TState>(IGameStateMachine to, bool rebindIfExists = false) where TState : IState
        {
            TState state = _diContainer.Instantiate<TState>();

            if (rebindIfExists)
                to.AddStateWithRemove(state);
            else
                to.AddState(state);
        }
    }
}