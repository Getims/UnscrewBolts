using System;
using System.Collections.Generic;
using Scripts.Configs.Levels;
using Scripts.Data;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.UI.Factories;
using UnityEngine;

namespace Scripts.UI.MainMenu.Levels
{
    [Serializable]
    public class ButtonsCreator
    {
        [SerializeField]
        private LevelButton _levelButtonPrefab;

        [SerializeField]
        private Transform _buttonsContainer;

        [SerializeField]
        private List<LevelButton> _createdButtons = new List<LevelButton>();

        private int _buttonsCount = 0;
        private int _levelsCount = 0;
        private IProgressDataService _progressDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private IUIElementFactory _uiElementFactory;

        public List<LevelButton> CreatedButtons => _createdButtons;
        public int ButtonsCount => _buttonsCount;

        public void Initialize(IUIElementFactory uiElementFactory, IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService)
        {
            _uiElementFactory = uiElementFactory;
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _progressDataService = progressDataService;
        }

        public void CreateButtons(Action<int> onLevelClick)
        {
            _levelsCount = _gameLevelsConfigProvider.LevelsCount;
            _buttonsCount = 0;
            LevelData levelData = null;
            PrizeConfig prizeConfig = null;

            for (int i = 0; i < _levelsCount; i++)
            {
                levelData = _progressDataService.GetLevelData(i);
                prizeConfig = _gameLevelsConfigProvider.GetLevel(i).PrizeConfig;

                if (i <= _createdButtons.Count - 1)
                {
                    _createdButtons[i].Initialize(i, prizeConfig, levelData);
                    _createdButtons[i].OnClick += onLevelClick;
                }
                else
                {
                    LevelButton levelButton = _uiElementFactory.Create(_levelButtonPrefab, _buttonsContainer);
                    levelButton.name = $"Level_{i + 1}_(Button)";
                    levelButton.Initialize(i, prizeConfig, levelData);
                    levelButton.OnClick += onLevelClick;

                    _createdButtons.Add(levelButton);
                }

                _createdButtons[i].gameObject.SetActive(true);
                _buttonsCount++;
            }
        }
    }
}