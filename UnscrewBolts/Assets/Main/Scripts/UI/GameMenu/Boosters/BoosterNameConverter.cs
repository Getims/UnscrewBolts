using Scripts.Core.Enums;
using Scripts.Core.Utilities;

namespace Scripts.UI.GameMenu.Boosters
{
    internal static class BoosterNameConverter
    {
        public static string GetBoosterName(BoosterType boosterType)
        {
            Utils.ReworkPoint("Add booster translation");
            switch (boosterType)
            {
                case BoosterType.Restart:
                    return $"Restart";
                case BoosterType.UnlockAnchor:
                    return $"Unlock hole";
                case BoosterType.UnscrewBolt:
                    return $"Unscrew bolt";
                case BoosterType.Bomb:
                    return $"Bomb";
                default:
                    return $"Not setuped";
            }
        }
    }
}