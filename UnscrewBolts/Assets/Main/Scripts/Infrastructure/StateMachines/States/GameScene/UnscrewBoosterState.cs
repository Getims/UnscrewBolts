using Scripts.GameLogic.GameFlow;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.UI.Factories;
using Scripts.UI.GameMenu;
using Scripts.UI.GameMenu.Boosters;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class UnscrewBoosterState : IEnterState, IExitState
    {
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly IGameFlowProvider _gameFlowProvider;
        private readonly IBoltMediator _boltMediator;
        private readonly LocalEventProvider _localEventProvider;
        private readonly GameStateMachine _stateMachine;

        private TopGamePanel _topGamePanel;
        private BoostersPanel _boostersPanel;

        public UnscrewBoosterState(GameStateMachine stateMachine, IUIMenuFactory uiMenuFactory,
            IGameFlowProvider gameFlowProvider,
            IBoltMediator boltMediator, LocalEventProvider localEventProvider)
        {
            _stateMachine = stateMachine;
            _localEventProvider = localEventProvider;
            _boltMediator = boltMediator;
            _gameFlowProvider = gameFlowProvider;
            _uiMenuFactory = uiMenuFactory;
        }

        public void Enter()
        {
            if (_topGamePanel == null)
                _topGamePanel = _uiMenuFactory.GetPanel<TopGamePanel>();
            if (_boostersPanel == null)
                _boostersPanel = _uiMenuFactory.GetPanel<BoostersPanel>();

            _topGamePanel.Hide();
            _boostersPanel.Hide();
            _gameFlowProvider.StopTimer();
            _boltMediator.UnselectCurrentBolt();

            _localEventProvider.AddListener<RemoveBoltEvent>(OnBoltRemove);
            _localEventProvider.Invoke<UnscrewBoosterUseEvent, bool>(true);
        }

        public void Exit()
        {
            _localEventProvider.RemoveListener<RemoveBoltEvent>(OnBoltRemove);
            _localEventProvider.Invoke<UnscrewBoosterUseEvent, bool>(false);
        }

        private void OnBoltRemove()
        {
            _topGamePanel.Show();
            _boostersPanel.Show();
            _gameFlowProvider.StartTimer();
            _stateMachine.Enter<GamePlayState>();
        }
    }
}