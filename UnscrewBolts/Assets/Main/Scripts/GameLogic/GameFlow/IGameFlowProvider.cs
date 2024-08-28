namespace Scripts.GameLogic.GameFlow
{
    public interface IGameFlowProvider
    {
        float RemainLevelTime { get; }
        bool IsTimeTracking { get; }
        bool IsLoaded { get; }

        void Initialize();
        void ShowLevel();
        void HideLevel();
        void StopTimer();
        void StartTimer();
        void AddTimeToTimer(int time);
    }
}