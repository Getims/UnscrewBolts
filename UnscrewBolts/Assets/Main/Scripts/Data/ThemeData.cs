using System;
using System.Collections.Generic;
using Scripts.Data.Core;

namespace Scripts.Data
{
    [Serializable]
    public class ThemeData : GameData
    {
        public string CurrentThemeID = string.Empty;
        public List<string> UnlockedThemesID = new List<string>();
    }
}