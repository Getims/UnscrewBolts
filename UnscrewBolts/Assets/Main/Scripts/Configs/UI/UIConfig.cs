using System;
using Scripts.Configs.Core;
using UnityEngine;

namespace Scripts.Configs.UI
{
    [Serializable]
    public class UIConfig : ScriptableConfig
    {
        [SerializeField]
        private UIListConfig _mainMenuListConfig;

        [SerializeField]
        private UIListConfig _gameListConfig;

        public UIListConfig MainMenuListConfig => _mainMenuListConfig;
        public UIListConfig GameListConfig => _gameListConfig;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.UI_CATEGORY;
#endif
    }
}