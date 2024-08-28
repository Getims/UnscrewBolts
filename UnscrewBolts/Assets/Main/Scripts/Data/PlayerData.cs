using System;
using Scripts.Data.Core;

namespace Scripts.Data
{
    [Serializable]
    public class PlayerData : GameData
    {
        public bool IsSoundOn = true;
        public bool IsMusicOn = true;
        public int Money;
    }
}