using Scripts.Configs.Core;
using Scripts.Core.Enums;
using Scripts.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Player
{
    public class ThemeConfig : ScriptableConfig
    {
        [SerializeField, ReadOnly]
        private string _themeID = Utils.GetUniqueID(8);

        [SerializeField]
        private Sprite _background;

        [SerializeField]
        private Sprite _shopIcon;

        [SerializeField]
        private CurrencyType _unlockType = CurrencyType.Free;

        [SerializeField]
        [ShowIf(nameof(NeedAmount))]
        private int _moneyAmount;

        public string ThemeId => _themeID;
        public Sprite Background => _background;
        public Sprite ShopIcon => _shopIcon;
        public CurrencyType UnlockType => _unlockType;
        public int MoneyAmount => NeedAmount ? _moneyAmount : 0;

        private bool NeedAmount =>
            _unlockType != CurrencyType.AdsWatch &&
            _unlockType != CurrencyType.Free;

        [Button]
        private void GenerateThemeID() =>
            _themeID = Utils.GetUniqueID(8);

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.PLAYER_CATEGORY;
#endif
    }
}