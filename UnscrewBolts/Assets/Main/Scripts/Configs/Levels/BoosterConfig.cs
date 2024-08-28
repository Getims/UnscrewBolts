using Scripts.Configs.Core;
using Scripts.Core.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Levels
{
    public class BoosterConfig : ScriptableConfig
    {
        [SerializeField]
        private BoosterType _boosterType;

        [SerializeField]
        [PreviewField(Height = 100, Alignment = ObjectFieldAlignment.Left)]
        private Sprite _boosterIcon;

        [SerializeField, Min(0)]
        private int _moneyCost;

        [SerializeField, Min(1)]
        private int _unlockAtLevel = 1;

        public BoosterType BoosterType => _boosterType;
        public Sprite BoosterIcon => _boosterIcon;
        public int MoneyCost => _moneyCost;
        public int UnlockAtLevel => _unlockAtLevel;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}