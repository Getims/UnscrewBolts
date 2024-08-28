using Scripts.Data.Core;
using Scripts.Infrastructure.Providers.Events;

namespace Scripts.Data.Services
{
    public interface IProgressDataService
    {
        int CurrentLevel { get; }
        int LevelsCount { get; }
        int UnlockedLevelsCount { get; }
        bool HasReward { get; }
        int CurrentLevelStep { get; }
        bool IsTutorialComplete { get; }

        LevelData GetLevelData(int id);
        void AddLevelData(bool autosave = true);
        void SetLevelState(int id, bool isUnlocked, bool isComplete, bool autosave = true);
        void SaveData();
        void SetCurrentLevel(int levelId, bool autosave = true);
        void SetRewardState(bool hasReward, bool autosave = true);
        void SwitchToNextLevel(bool autosave = true);
        void SetLevelStep(int step, bool autosave = true);
        void SetTutorialState(bool isTutorialComplete, bool autosave = true);
    }

    public class ProgressDataService : DataService, IProgressDataService
    {
        private readonly ProgressData _progressData;
        private GlobalEventProvider _globalEventProvider;

        public int CurrentLevel => _progressData.CurrentLevel;
        public int CurrentLevelStep => _progressData.CurrentLevelStep;
        public int LevelsCount => _progressData.Levels.Count;
        public int UnlockedLevelsCount => CalculateUnlockedLevelsCount();
        public bool HasReward => _progressData.HasReward;
        public bool IsTutorialComplete => _progressData.IsTutorialComplete;

        protected ProgressDataService(IDatabase database, GlobalEventProvider globalEventProvider) : base(database)
        {
            _globalEventProvider = globalEventProvider;
            _progressData = database.GetData<ProgressData>();
        }

        public void SetCurrentLevel(int levelId, bool autosave = true)
        {
            if (levelId < 0)
                levelId = 0;

            if (levelId >= LevelsCount)
                levelId = LevelsCount - 1;

            _progressData.CurrentLevel = levelId;
            _globalEventProvider.Invoke<LevelSwitchEvent, int>(levelId);
            TryToSave(autosave);
        }

        public void SetLevelStep(int step, bool autosave = true)
        {
            if (step < 0)
                step = 0;

            if (step >= 1)
                step = 1;

            _progressData.CurrentLevelStep = step;
            _globalEventProvider.Invoke<LevelStepSwitchEvent, int>(step);
            TryToSave(autosave);
        }

        public void SwitchToNextLevel(bool autosave = true)
        {
            SetLevelState(CurrentLevel, true, true, false);
            int nextLevel = CurrentLevel + 1;
            if (nextLevel >= LevelsCount)
                return;

            LevelData levelData = GetLevelData(nextLevel);
            SetLevelState(nextLevel, true, levelData.IsLevelComplete(), false);
            SetCurrentLevel(nextLevel, false);

            TryToSave(autosave);
        }

        public LevelData GetLevelData(int id)
        {
            if (id < 0)
                id = 0;

            if (id < LevelsCount)
                return _progressData.Levels[id];

            return _progressData.Levels[0];
        }

        public void AddLevelData(bool autosave = true)
        {
            int id = LevelsCount;
            _progressData.Levels.Add(new LevelData(id));

            TryToSave(autosave);
        }

        public void SetRewardState(bool hasReward, bool autosave = true)
        {
            _progressData.HasReward = hasReward;
            TryToSave(autosave);
        }
        
        public void SetTutorialState(bool isTutorialComplete, bool autosave = true)
        {
            _progressData.IsTutorialComplete = isTutorialComplete;
            TryToSave(autosave);
        }

        public void SetLevelState(int id, bool isUnlocked, bool isComplete, bool autosave = true)
        {
            if (id < 0 || id >= LevelsCount)
                return;

            var levelData = _progressData.Levels[id];
            levelData.SetLockState(isUnlocked);
            levelData.SetCompleteState(isComplete);

            TryToSave(autosave);
        }

        public void SaveData() => TryToSave(true);

        private int CalculateUnlockedLevelsCount()
        {
            int levelsCount = LevelsCount;
            int i = 0;
            for (; i < levelsCount; i++)
            {
                if (!_progressData.Levels[i].IsUnlocked)
                    return i;
            }

            return i;
        }
    }
}