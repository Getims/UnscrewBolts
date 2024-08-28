using System;
using Scripts.Core.Utilities;

namespace Scripts.Infrastructure.Ads
{
    public static class AdsManager
    {
        private static Action<bool> _onRewarded;

        public static void ShowRewarded(Action<bool> onRewarded)
        {
            _onRewarded = onRewarded;
            Utils.ReworkPoint("Reward logic");
            OnRewarded(true);
        }

        private static void OnRewarded(bool giveReward) =>
            _onRewarded?.Invoke(giveReward);
    }
}