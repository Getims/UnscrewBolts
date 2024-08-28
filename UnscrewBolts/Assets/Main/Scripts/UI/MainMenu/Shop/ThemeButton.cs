using System;
using Scripts.Configs.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Shop
{
    public class ThemeButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _themeIcon;

        [SerializeField]
        private Image _selectOutline;

        [SerializeField]
        private Image _checkmark;

        [SerializeField]
        private Image _lockIcon;

        [SerializeField]
        private Image _lockShade;

        private bool _isLocked;
        private ThemeConfig _themeConfig;

        public string ThemeId => _themeConfig.ThemeId;
        public event Action<ThemeConfig> OnClick;

        public void Initialize(ThemeConfig themeConfig)
        {
            _themeConfig = themeConfig;
            _themeIcon.sprite = themeConfig.ShopIcon;
        }

        public void UpdateState(string _currentThemeId, string _selectedThemeId, bool _isUnlocked)
        {
            _selectOutline.enabled = _selectedThemeId.Equals(_themeConfig.ThemeId);
            _checkmark.enabled = _currentThemeId.Equals(_themeConfig.ThemeId);
            _lockIcon.enabled = !_isUnlocked;
            _lockShade.enabled = !_isUnlocked;
        }

        private void Start() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick() =>
            OnClick?.Invoke(_themeConfig);
    }
}