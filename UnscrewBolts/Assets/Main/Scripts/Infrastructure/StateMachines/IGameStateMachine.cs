using System;
using Scripts.Infrastructure.StateMachines.BaseStates;

namespace Scripts.Infrastructure.StateMachines
{
    public interface IGameStateMachine
    {
        void AddState<T>(T state) where T : IState;
        void AddStateWithRemove<T>(T state) where T : IState;
        void RemoveState(Type type);
        void Enter<TState>() where TState : class, IEnterState;
        void Enter<TState, TParameter>(TParameter payload) where TState : class, IEnterState<TParameter>;
        void FixedTick();
        void LateTick();
        void Tick();
    }
}