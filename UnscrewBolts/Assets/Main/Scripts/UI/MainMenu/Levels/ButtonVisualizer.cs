using System;
using Scripts.Configs.Levels;
using Scripts.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Levels
{
    [Serializable]
    public class ButtonVisualizer
    {
        [SerializeField]
        private Transform _buttonContainer;

        [SerializeField, Min(0)]
        private float _xOffset = 0;

        [SerializeField]
        private Image _lockIcon;

        [SerializeField]
        private Image _selectedOutline;

        [SerializeField]
        private Image _prizeIcon;

        [SerializeField]
        private TextMeshProUGUI _value;

        [SerializeField]
        private Image _checkMark;

        public void SetNumber(int number)
        {
            _value.text = (number + 1).ToString();
            SetOffset(number);
        }

        public void SetState(LevelState levelState, bool isComplete)
        {
            _checkMark.enabled = isComplete;
            switch (levelState)
            {
                case LevelState.Locked:
                    SetSelected(false);
                    SetLock(true);
                    break;
                case LevelState.Unlocked:
                    SetLock(false);
                    SetSelected(false);
                    if (isComplete)
                        SetPrize(false);
                    break;
                case LevelState.Selected:
                    SetLock(false);
                    SetSelected(true);
                    if (isComplete)
                        SetPrize(false);
                    break;
            }
        }

        public void SetPrize(PrizeConfig prizeConfig)
        {
            _prizeIcon.enabled = prizeConfig.HasPrize;
            _prizeIcon.sprite = prizeConfig.PrizeIcon;
        }

        private void SetPrize(bool enabled)
        {
            _prizeIcon.enabled = enabled;
        }

        private void SetSelected(bool selected) =>
            _selectedOutline.enabled = selected;

        private void SetLock(bool locked)
        {
            _lockIcon.enabled = locked;
            _value.enabled = !locked;
        }

        private void SetOffset(int number)
        {
            Vector3 newPosition = _buttonContainer.localPosition;
            if (number % 2 == 1)
                newPosition.x = -_xOffset;
            else
                newPosition.x = _xOffset;

            _buttonContainer.localPosition = newPosition;
        }
    }
}