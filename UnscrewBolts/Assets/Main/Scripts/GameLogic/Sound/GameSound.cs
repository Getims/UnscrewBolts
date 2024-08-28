using System;

namespace Scripts.GameLogic.Sound
{
    internal class GameSound
    {
        private readonly string _id;
        private long _lastPlayTime;

        public string Id => _id;
        public long LastPlayTime => _lastPlayTime;

        public GameSound(string id, long time = 0)
        {
            _id = id;
            _lastPlayTime = time;
        }

        public void SetLastPlayTime(long time)
        {
            time = Math.Max(time, 0);
            _lastPlayTime = time;
        }
    }
}