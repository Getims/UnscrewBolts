using Scripts.Configs.Levels;
using Scripts.Data.Services;
using Scripts.GameLogic.Background;
using Scripts.GameLogic.Levels;
using Scripts.GameLogic.Levels.Bolts;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.GameFlow
{
    public class GameFlowController : MonoBehaviour, IGameFlowProvider, IBoltMediator
    {
        [SerializeField]
        private BackgroundController _backgroundController;

        [SerializeField]
        private Transform _levelContainer;

        private IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private IProgressDataService _progressDataService;
        private LocalEventProvider _localEventProvider;

        private Bolt _selectedBolt;
        private LevelTimeTracker _levelTimeTracker = new LevelTimeTracker();
        private LevelConfig _levelConfig;
        private LevelController _currentLevel;
        private bool _isLoaded = false;

        public float RemainLevelTime => _levelTimeTracker.RemainTime;
        public bool IsTimeTracking => _levelTimeTracker.IsTracking;
        public bool IsLoaded => _isLoaded;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService,
            LocalEventProvider localEventProvider)
        {
            _localEventProvider = localEventProvider;
            _progressDataService = progressDataService;
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
        }

        public void Initialize()
        {
            _backgroundController.Initialize();

            int currentLevel = _progressDataService.CurrentLevel;
            int currentStep = _progressDataService.CurrentLevelStep;
            _levelConfig = _gameLevelsConfigProvider.GetLevel(currentLevel);

            StepConfig stepConfig = _levelConfig.GetStepConfig(currentStep);
            _levelTimeTracker.Initialize(stepConfig.Time);
            _levelTimeTracker.OnTimeEndEvent += OnTimeEnd;

            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);

            _currentLevel = Instantiate(stepConfig.LevelPrefab, _levelContainer);
            _currentLevel.Initialize();
            _currentLevel.NoElementsEvent += OnNoElements;
            _currentLevel.Hide(true);
        }

        public Bolt GetSelectedBolt() =>
            _selectedBolt;

        public void SetSelectedBolt(Bolt bolt) =>
            _selectedBolt = bolt;

        public void UnselectCurrentBolt()
        {
            if (_selectedBolt == null)
                return;
            _selectedBolt.SetScrewed();
            _selectedBolt = null;
        }

        public void ShowLevel() =>
            _currentLevel.Show();

        public void HideLevel() =>
            _currentLevel.Hide(false);

        public void StopTimer() =>
            _levelTimeTracker.StopTracking();

        public void StartTimer() =>
            _levelTimeTracker.TrackTime();

        public void AddTimeToTimer(int time)
        {
            if (!_levelTimeTracker.IsTracking)
                _levelTimeTracker.TrackTime(time);
        }

        private void Start()
        {
            _isLoaded = true;
        }

        private void OnDestroy()
        {
            _levelTimeTracker.StopTracking();
            _levelTimeTracker.OnTimeEndEvent -= OnTimeEnd;
        }

        private void OnNoElements() => SetGameOver(true);
        private void OnTimeEnd() => SetGameOver(false);

        private void SetGameOver(bool isWin)
        {
            _levelTimeTracker.StopTracking();
            _localEventProvider.Invoke<GameOverEvent, bool>(isWin);
        }
    }
}