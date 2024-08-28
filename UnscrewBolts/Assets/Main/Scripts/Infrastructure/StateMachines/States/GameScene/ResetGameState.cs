using Scripts.Core.Enums;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class ResetGameState : IEnterState<GameResetType>
    {
        private readonly GlobalEventProvider _globalEventsProvider;
        private readonly IProgressDataService _progressDataService;

        public ResetGameState(GlobalEventProvider globalEventsProvider, IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _globalEventsProvider = globalEventsProvider;
        }

        public void Enter(GameResetType resetType)
        {
            switch (resetType)
            {
                case GameResetType.Full:
                    _progressDataService.SetLevelStep(0);
                    _globalEventsProvider.Invoke<ReloadLevelEvent>();
                    break;
                case GameResetType.CurrentStep:
                    _globalEventsProvider.Invoke<ReloadLevelEvent>();
                    break;
            }
        }
    }
}