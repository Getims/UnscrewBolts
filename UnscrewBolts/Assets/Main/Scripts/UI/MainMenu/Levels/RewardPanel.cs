using Scripts.UI.Base;
using TMPro;
using UnityEngine;

namespace Scripts.UI.MainMenu.Levels
{
    public class RewardPanel : UIPanel
    {
        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        [SerializeField]
        private Transform _coinIcon;

        public Vector3 CoinIconPosition => _coinIcon.position;

        public void SetValue(int value) =>
            _valueTMP.text = value.ToString();
    }
}