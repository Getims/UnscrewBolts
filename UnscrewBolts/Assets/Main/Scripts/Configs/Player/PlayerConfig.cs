using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Scripts.Configs.Core;
using UnityEngine;

namespace Scripts.Configs.Player
{
    public class PlayerConfig : ScriptableConfig
    {
        [SerializeField]
        private List<ThemeConfig> _themes = new List<ThemeConfig>();

        public ReadOnlyCollection<ThemeConfig> Themes => _themes.AsReadOnly();

        public ThemeConfig GetTheme(string themeId) =>
            _themes.FirstOrDefault(theme => theme.ThemeId.Equals(themeId));

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.PLAYER_CATEGORY;
#endif
    }
}