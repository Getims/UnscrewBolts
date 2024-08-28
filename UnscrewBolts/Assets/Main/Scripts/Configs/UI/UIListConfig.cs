using System;
using System.Collections.Generic;
using Scripts.Configs.Core;
using Scripts.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.UI
{
    [Serializable]
    public class UIListConfig : ScriptableConfig
    {
        [SerializeField, Required, AssetsOnly]
        private List<UIPanel> _prefabs = new List<UIPanel>();

        public List<UIPanel> Prefabs => _prefabs;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.UI_CATEGORY;
#endif
    }
}