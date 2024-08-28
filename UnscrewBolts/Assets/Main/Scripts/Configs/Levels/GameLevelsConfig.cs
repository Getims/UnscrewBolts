using System.Collections.Generic;
using Scripts.Configs.Core;
using UnityEngine;

namespace Scripts.Configs.Levels
{
    public class GameLevelsConfig : ScriptableConfig
    {
        [SerializeField]
        private List<LevelConfig> _levelConfigs = new List<LevelConfig>();

        [SerializeField]
        private List<BoosterConfig> _boostersConfigs = new List<BoosterConfig>();

        [SerializeField]
        private BoosterConfig _unlockAnchorBoosterConfig;

        public IEnumerable<LevelConfig> LevelConfigs => _levelConfigs;
        public IEnumerable<BoosterConfig> BoostersConfigs => _boostersConfigs;
        public BoosterConfig UnlockAnchorBoosterConfig => _unlockAnchorBoosterConfig;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}