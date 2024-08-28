using Scripts.Core.Enums;
using Scripts.Infrastructure.ScenesManager;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Global;

namespace Scripts.Infrastructure.StateMachines.States.Load
{
    public class LoadGameLoaderSceneState : LoadSceneState, IEnterState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadGameLoaderSceneState(GameStateMachine stateMachine, ISceneLoader sceneLoader) : base(sceneLoader) =>
            _stateMachine = stateMachine;

        public void Enter() =>
            base.Enter(Scenes.GameLoader, OnLoaded);

        private void OnLoaded() =>
            _stateMachine.Enter<GameLoaderState>();
    }
}