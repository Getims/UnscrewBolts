using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Levels
{
    [Serializable]
    public class PrizeConfig
    {
        [SerializeField]
        private bool _hasPrize = false;

        [SerializeField, EnableIf(nameof(_hasPrize))]
        private Sprite _prizeIcon;

        [SerializeField, EnableIf(nameof(_hasPrize))]
        private int _moneyReward;

        public bool HasPrize => _hasPrize;
        public Sprite PrizeIcon => _prizeIcon;
        public int MoneyReward => _moneyReward;
    }
}