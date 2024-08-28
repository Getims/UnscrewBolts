using System;
using DG.Tweening;

namespace Scripts.GameLogic.GameFlow
{
    public class LevelTimeTracker
    {
        private float _levelTime = 0;
        private float _currentTime = 0;
        private Tweener _timerTW;
        private bool _isTracking = false;

        public event Action OnTimeEndEvent;
        public float LevelTime => _levelTime;
        public float CurrentTimePercent => _currentTime;
        public float RemainTime => (1 - _currentTime) * _levelTime;
        public bool IsTracking => _isTracking;

        public void Initialize(float levelTime)
        {
            _isTracking = false;
            _levelTime = levelTime;
            _currentTime = 0;
            _timerTW?.Kill();
        }

        public void TrackTime(float levelTime)
        {
            Initialize(levelTime);
            TrackTime();
        }

        public void TrackTime()
        {
            if (_levelTime <= 0)
                return;

            _isTracking = true;
            _timerTW?.Kill();

            _timerTW = DOTween.To(() => _currentTime, x => _currentTime = x, 1, _levelTime)
                .SetEase(Ease.Linear)
                .OnComplete(OnTimeEnd);
        }

        public void StopTracking()
        {
            _isTracking = false;
            //_currentTime = 0;
            _timerTW?.Kill();
        }

        public void Reset()
        {
            _isTracking = true;
            _currentTime = 0;
            _timerTW?.Kill();

            _timerTW = DOTween.To(() => _currentTime, x => _currentTime = x, 1, _levelTime)
                .SetEase(Ease.Linear)
                .OnComplete(OnTimeEnd);
        }

        private void OnTimeEnd()
        {
            _isTracking = false;
            OnTimeEndEvent?.Invoke();
        }
    }
}