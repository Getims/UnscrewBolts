using Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Levels
{
    public class LevelConfig : ScriptableConfig
    {
        [SerializeField]
        private int _reward;

        [Title("Steps config")]
        [SerializeField]
        private StepConfig _firstStepConfig;

        [SerializeField]
        private StepConfig _secondStepConfig;

        [SerializeField]
        [Title("Prize config"), InlineProperty, HideLabel]
        private PrizeConfig _prizeConfig;

        public int Reward => _reward;
        public PrizeConfig PrizeConfig => _prizeConfig;

        public StepConfig GetStepConfig(int step)
        {
            if (step == 0)
                return _firstStepConfig;
            return _secondStepConfig;
        }

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}