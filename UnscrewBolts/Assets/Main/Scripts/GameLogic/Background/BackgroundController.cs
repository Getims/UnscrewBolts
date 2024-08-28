using System.Collections.Generic;
using Scripts.Configs.Player;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Background
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField]
        private List<SpriteRenderer> _sprites;

        private PlayerConfig _playerConfig;
        private IThemeDataService _themeDataService;
        private LocalEventProvider _localEventProvider;

        [Inject]
        public void Construct(IPlayerConfigProvider levelsConfigProvider, IThemeDataService themeDataService,
            LocalEventProvider localEventProvider)
        {
            _localEventProvider = localEventProvider;
            _themeDataService = themeDataService;
            _playerConfig = levelsConfigProvider.Config;
            _localEventProvider.AddListener<ThemeSwitchEvent>(OnThemeSwitched);
        }

        [Button]
        public void Initialize() =>
            UpdateTheme();

        private void OnDestroy() =>
            _localEventProvider.RemoveListener<ThemeSwitchEvent>(OnThemeSwitched);

        private void UpdateTheme()
        {
            string currentTheme = _themeDataService.CurrentThemeID;
            ThemeConfig themeConfig = _playerConfig.GetTheme(currentTheme);
            if (themeConfig == null)
                return;

            Sprite background = themeConfig.Background;
            foreach (SpriteRenderer sprite in _sprites)
                sprite.sprite = background;
        }

        private void OnThemeSwitched() =>
            UpdateTheme();
    }
}