namespace Scripts.Infrastructure.StateMachines.BaseStates
{
    public interface IState
    {
    }

    public interface IEnterState : IState
    {
        void Enter();
    }

    public interface IExitState : IState
    {
        void Exit();
    }

    public interface IEnterState<TParameter> : IState
    {
        void Enter(TParameter payload);
    }
}