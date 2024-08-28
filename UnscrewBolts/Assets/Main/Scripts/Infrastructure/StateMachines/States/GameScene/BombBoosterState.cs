using Scripts.GameLogic.GameFlow;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.StateMachines.BaseStates;
using Scripts.UI.Factories;
using Scripts.UI.GameMenu;
using Scripts.UI.GameMenu.Boosters;

namespace Scripts.Infrastructure.StateMachines.States.GameScene
{
    internal class BombBoosterState : IEnterState, IExitState
    {
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly IGameFlowProvider _gameFlowProvider;
        private readonly LocalEventProvider _localEventProvider;
        private readonly GameStateMachine _stateMachine;

        private TopGamePanel _topGamePanel;
        private BoostersPanel _boostersPanel;

        public BombBoosterState(GameStateMachine stateMachine, IUIMenuFactory uiMenuFactory,
            IGameFlowProvider gameFlowProvider, LocalEventProvider localEventProvider)
        {
            _stateMachine = stateMachine;
            _localEventProvider = localEventProvider;
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

            _localEventProvider.AddListener<BombExplodeEvent>(OnBoltRemove);
            _localEventProvider.Invoke<BombBoosterUseEvent>();
        }

        public void Exit()
        {
            _localEventProvider.RemoveListener<RemoveBoltEvent>(OnBoltRemove);
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