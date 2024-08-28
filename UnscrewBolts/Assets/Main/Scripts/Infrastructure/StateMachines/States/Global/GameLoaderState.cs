using Scripts.Configs;
using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.Infrastructure.StateMachines.States.Load;

namespace Scripts.Infrastructure.StateMachines.States.Global
{
    public class GameLoaderState : IEnterState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGlobalConfigProvider _globalConfigProvider;
        private readonly GlobalEventProvider _globalEventsProvider;
        private readonly IProgressDataService _progressDataService;

        public GameLoaderState(IGameStateMachine stateMachine, IGlobalConfigProvider globalConfigProvider,
            GlobalEventProvider globalEventsProvider, IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _globalEventsProvider = globalEventsProvider;
            _stateMachine = stateMachine;
            _globalConfigProvider = globalConfigProvider;
            _globalEventsProvider.AddListener<GameLoadCompleteEvent>(MoveToNextState);
        }

        public void Enter()
        {
        }

        private void MoveToNextState()
        {
            _globalEventsProvider.RemoveListener<GameLoadCompleteEvent>(MoveToNextState);
            GlobalConfig config = _globalConfigProvider.Config;
            EnterSceneState(config.StartScene);
        }

        private void EnterSceneState(Scenes scene)
        {
            switch (scene)
            {
                case Scenes.GameLoader:
                case Scenes.NULL:
                case Scenes.MainMenu:
                    if (_progressDataService.CurrentLevel == 0 && !_progressDataService.IsTutorialComplete)
                        _stateMachine.Enter<LoadGameSceneState>();
                    else
                        _stateMachine.Enter<LoadMainMenuSceneState>();
                    break;
                case Scenes.Game:
                    _stateMachine.Enter<LoadGameSceneState>();
                    break;
            }
        }
    }
}