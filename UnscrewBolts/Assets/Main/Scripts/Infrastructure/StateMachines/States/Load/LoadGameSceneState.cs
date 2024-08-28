using Scripts.Core.Enums;
using Scripts.Infrastructure.ScenesManager;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Global;

namespace Scripts.Infrastructure.StateMachines.States.Load
{
    public class LoadGameSceneState : LoadSceneState, IEnterState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadGameSceneState(GameStateMachine stateMachine, ISceneLoader sceneLoader) : base(sceneLoader) =>
            _stateMachine = stateMachine;

        public void Enter() =>
            base.Enter(Scenes.Game, OnLoaded);

        private void OnLoaded() =>
            _stateMachine.Enter<GameLoopState>();
    }
}