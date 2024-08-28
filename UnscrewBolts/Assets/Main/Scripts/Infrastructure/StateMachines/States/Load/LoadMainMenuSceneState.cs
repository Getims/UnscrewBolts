using Scripts.Core.Enums;
using Scripts.Infrastructure.ScenesManager;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Global;

namespace Scripts.Infrastructure.StateMachines.States.Load
{
    public class LoadMainMenuSceneState : LoadSceneState, IEnterState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadMainMenuSceneState(GameStateMachine stateMachine, ISceneLoader sceneLoader) : base(sceneLoader) =>
            _stateMachine = stateMachine;

        public void Enter() =>
            base.Enter(Scenes.MainMenu, OnLoaded);

        private void OnLoaded() =>
            _stateMachine.Enter<MainMenuState>();
    }
}