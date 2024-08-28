using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Configs.Levels;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.UI.Base;
using Scripts.UI.Common.UIAnimator.Main;
using UnityEngine;
using Zenject;

namespace Scripts.UI.GameMenu.Boosters
{
    public class BoostersPanel : UIPanel
    {
        [SerializeField]
        private List<BoostersButton> _boostersButtons = new List<BoostersButton>();

        [SerializeField]
        private UIAnimator _showAnimator;

        [SerializeField]
        private UIAnimator _hideAnimator;

        private IEnumerable<BoosterConfig> _boosterConfigs;
        private int _currentLevel;
        private int _unlockedLevelsCount;

        public new float FadeTime => base.FadeTime > _showAnimator.GetAnimatorWorkTime()
            ? base.FadeTime
            : _showAnimator.GetAnimatorWorkTime();

        public event Action<BoosterConfig> OnBoosterSelect;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService)
        {
            _boosterConfigs = gameLevelsConfigProvider.Config.BoostersConfigs;
            _currentLevel = progressDataService.CurrentLevel;
            _unlockedLevelsCount = progressDataService.UnlockedLevelsCount;
        }

        public void Initialize() => SetupButtons();

        public override void Show()
        {
            base.Show();
            _hideAnimator.Stop();
            _showAnimator.Play();
        }

        public override void Hide()
        {
            base.Hide();
            _showAnimator.Stop();
            _hideAnimator.Play();
        }

        private void SetupButtons()
        {
            int i = 0;
            foreach (BoostersButton boostersButton in _boostersButtons)
            {
                boostersButton.Initialize(_boosterConfigs.ElementAt(i), _currentLevel, _unlockedLevelsCount, OnBoosterButtonClick);
                i++;
            }
        }

        private void OnBoosterButtonClick(BoosterConfig boosterConfig) =>
            OnBoosterSelect?.Invoke(boosterConfig);
    }
}