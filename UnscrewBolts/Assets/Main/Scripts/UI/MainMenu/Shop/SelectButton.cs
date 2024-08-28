using System;
using Scripts.Configs.Player;
using Scripts.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Shop
{
    public class SelectButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _selectContainer;

        [SerializeField]
        private GameObject _unlockMoneyContainer;

        [SerializeField]
        private GameObject _unlockAdsContainer;

        [SerializeField]
        private TextMeshProUGUI _moneyValueTMP;

        public event Action OnClick;

        public void Initialize(ThemeConfig themeConfig, bool isThemeUnlocked)
        {
            bool moneyState = !isThemeUnlocked && themeConfig.UnlockType == CurrencyType.Money;
            bool adsState = !isThemeUnlocked && themeConfig.UnlockType == CurrencyType.AdsWatch;

            SetMoneyValue(themeConfig.MoneyAmount);
            SetSelectState(isThemeUnlocked || themeConfig.UnlockType == CurrencyType.Free);
            SetMoneyState(moneyState);
            SetAdsState(adsState);
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void SetMoneyValue(int amount) =>
            _moneyValueTMP.text = amount.ToString();

        private void SetSelectState(bool selected) =>
            _selectContainer.gameObject.SetActive(selected);

        private void SetMoneyState(bool showMoney) =>
            _unlockMoneyContainer.gameObject.SetActive(showMoney);

        private void SetAdsState(bool showAds) =>
            _unlockAdsContainer.gameObject.SetActive(showAds);

        private void OnButtonClick() => OnClick?.Invoke();
    }
}