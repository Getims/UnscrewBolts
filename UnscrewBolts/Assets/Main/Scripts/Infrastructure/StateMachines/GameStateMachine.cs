using System;
using System.Collections.Generic;
using Scripts.Infrastructure.StateMachines.BaseStates;
using UnityEngine;

namespace Scripts.Infrastructure.StateMachines
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine()
        {
            _states = new Dictionary<Type, IState>();
        }

        public void AddState<TState>(TState state) where TState : IState
        {
            Type type = state.GetType();

            if (_states.ContainsKey(type))
                return;

            _states.Add(type, state);
        }

        public void AddStateWithRemove<TState>(TState state) where TState : IState
        {
            Type type = state.GetType();

            if (_states.ContainsKey(type))
                RemoveState(type);

            _states.Add(type, state);
        }

        public void RemoveState(Type type) =>
            _states.Remove(type);

        public void Enter<TState>() where TState : class, IEnterState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TParameter>(TParameter payload) where TState : class, IEnterState<TParameter>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void FixedTick()
        {
            if (_currentState is IFixedTickableState state)
                state.FixedTick();
        }

        public void LateTick()
        {
            if (_currentState is ILateTickableState state)
                state.LateTick();
        }

        public void Tick()
        {
            if (_currentState is ITickableState state)
                state.Tick();
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            Type type = typeof(TState);

            if (!IsStateExists(type))
                return null;

            (_currentState as IExitState)?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class =>
            _states[typeof(TState)] as TState;

        private bool IsStateExists(Type type)
        {
            bool isStateExists = _states.ContainsKey(type);

            if (!isStateExists)
                LogStateDontExistsError(type);

            return isStateExists;
        }

        private static void LogStateDontExistsError(Type type)
        {
            string log = $"State of type ({type}) don't exists!";
            Debug.LogError(log);
        }
    }
}