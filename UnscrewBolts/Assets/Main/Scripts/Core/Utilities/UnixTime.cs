using System;

namespace Scripts.Core.Utilities
{
    public struct UnixTime
    {
        private const int HOURS_AT_DAY = 24;
        private const int MINUTES_IN_HOUR = 60;
        private const int SECONDS_IN_MINUTE = 60;

        private int _unixSeconds;
        private DateTime _dateTime;
        private DateTimeOffset _dateTimeOffset;

        public static int Now =>
            CurrentUnixTimeSeconds();

        public static UnixTime Today =>
            new UnixTime(CurrentUnixTimeSeconds());

        public int Second => _dateTime.Second;
        public int Minute => _dateTime.Minute;
        public int Hour => _dateTime.Hour;
        public int Day => _dateTime.Day;
        public int Month => _dateTime.Month;
        public int year => _dateTime.Year;

        public UnixTime(int unixSeconds = 0)
        {
            this._unixSeconds = unixSeconds;
            _dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(this._unixSeconds).ToLocalTime();
            _dateTime = _dateTimeOffset.DateTime;
        }

        public static int DaysToSeconds(int days) => 
            days * HOURS_AT_DAY * MINUTES_IN_HOUR * SECONDS_IN_MINUTE;

        public static int HoursToSeconds(int hours) => 
            hours * MINUTES_IN_HOUR * SECONDS_IN_MINUTE;

        public static int MinutesToSeconds(int minutes) => 
            minutes * SECONDS_IN_MINUTE;

        public int ToInt() => 
            _unixSeconds;

        public new string ToString() => 
            _dateTime.ToString();

        public int AddDays(int days)
        {
            _unixSeconds += DaysToSeconds(days);
            return _unixSeconds;
        }

        static int CurrentUnixTimeSeconds()
        {
            return (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}