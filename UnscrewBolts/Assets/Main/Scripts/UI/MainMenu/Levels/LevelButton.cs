using System;
using Scripts.Configs.Levels;
using Scripts.Core.Enums;
using Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Levels
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private ButtonVisualizer _visualizer;

        private int _levelNumber = -1;
        private bool _isLocked;
        private LevelData _levelData;

        public event Action<int> OnClick;

        public void Initialize(int number, PrizeConfig prizeConfig, LevelData levelData)
        {
            _levelData = levelData;
            _levelNumber = number;
            _visualizer.SetNumber(number);
            _visualizer.SetPrize(prizeConfig);
        }

        public void UpdateState(int currentLevel)
        {
            bool isCurrentLevel = _levelNumber == currentLevel;

            if (isCurrentLevel)
                _visualizer.SetState(LevelState.Selected, _levelData.IsLevelComplete());
            else
            {
                LevelState levelState = _levelData.IsUnlocked ? LevelState.Unlocked : LevelState.Locked;
                _isLocked = levelState == LevelState.Locked;
                _visualizer.SetState(levelState, _levelData.IsLevelComplete());
            }
        }

        private void Start() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            if (_isLocked || _levelNumber < 0)
                return;

            OnClick?.Invoke(_levelNumber);
        }
    }
}