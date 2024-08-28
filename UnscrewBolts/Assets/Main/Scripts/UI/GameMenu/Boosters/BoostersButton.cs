using System;
using Scripts.Configs.Levels;
using Scripts.Core.Utilities;
using Scripts.UI.Common.UIAnimator.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameMenu.Boosters
{
    public class BoostersButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private GameObject _lockContainer;

        [SerializeField]
        private TextMeshProUGUI _unlockInfo;

        [SerializeField]
        private UIAnimator _unlockInfoAnimator;

        private BoosterConfig _boosterConfig;
        private Action<BoosterConfig> _onBoosterButtonClick;
        private bool _isLocked;

        public void Initialize(BoosterConfig boosterConfig, int currentLevel, int unlockedLevelsCount,
            Action<BoosterConfig> onBoosterButtonClick)
        {
            _onBoosterButtonClick = onBoosterButtonClick;
            _boosterConfig = boosterConfig;
            _icon.sprite = _boosterConfig.BoosterIcon;

            SetInfo(_boosterConfig.UnlockAtLevel);

            _isLocked = currentLevel < _boosterConfig.UnlockAtLevel - 1;
            _lockContainer.SetActive(_isLocked);
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _unlockInfoAnimator.Stop();
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnClick);

        private void SetInfo(int unlockAtLevel)
        {
            Utils.ReworkPoint("Add booster info translation");
            string info = $"Unlock at level {unlockAtLevel}";
            _unlockInfo.text = info;
        }

        private void OnClick()
        {
            if (_isLocked)
                _unlockInfoAnimator.Play();
            else
                _onBoosterButtonClick?.Invoke(_boosterConfig);
        }
    }
}