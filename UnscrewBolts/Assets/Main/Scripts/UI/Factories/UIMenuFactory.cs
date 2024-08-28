using System.Collections.Generic;
using Scripts.Configs.UI;
using Scripts.Core.Utilities;
using Scripts.UI.Base;
using UnityEngine;
using Zenject;

namespace Scripts.UI.Factories
{
    public interface IUIMenuFactory
    {
        TPanel Create<TPanel>() where TPanel : UIPanel;
        TPanel Create<TPanel>(Transform parent) where TPanel : UIPanel;
        TPanel GetPanel<TPanel>() where TPanel : UIPanel;
    }

    public class UIMenuFactory : IUIMenuFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Transform _uiContainer;

        private Dictionary<string, UIPanel> _uiPanels = new Dictionary<string, UIPanel>();
        private Dictionary<string, UIPanel> _createdPanels = new Dictionary<string, UIPanel>();

        public UIMenuFactory(DiContainer diContainer, UIListConfig uiListConfig, Transform uiContainer = null)
        {
            _diContainer = diContainer;
            _uiContainer = uiContainer;
            SetupDictionary(uiListConfig);
        }

        public TPanel Create<TPanel>() where TPanel : UIPanel =>
            CreatePanel<TPanel>(null);

        public TPanel Create<TPanel>(Transform parent) where TPanel : UIPanel =>
            CreatePanel<TPanel>(parent);

        public TPanel GetPanel<TPanel>() where TPanel : UIPanel
        {
            string type = typeof(TPanel).Name;
            _createdPanels.TryGetValue(type, out UIPanel panel);
            if (panel == null)
                panel = CreatePanel<TPanel>(null);

            return (TPanel) panel;
        }

        private TPanel CreatePanel<TPanel>(Transform parent) where TPanel : UIPanel
        {
            string type = typeof(TPanel).Name;
            TPanel panelPrefab = GetPanelPrefab<TPanel>(type);
            if (panelPrefab == null)
                LogPanelNotFound(type);

            parent = parent == null ? _uiContainer : parent;
            TPanel createdPanel;
            if (parent == null)
                createdPanel = _diContainer.InstantiatePrefabForComponent<TPanel>(panelPrefab);
            else
                createdPanel = _diContainer.InstantiatePrefabForComponent<TPanel>(panelPrefab, parent);

            SavePanel(type, createdPanel);
            return createdPanel;
        }

        private void SetupDictionary(UIListConfig uiListConfig)
        {
            foreach (var uiPanel in uiListConfig.Prefabs)
            {
                string type = uiPanel.GetType().Name;
                AddPanel(type, uiPanel);
            }
        }

        private void AddPanel(string type, UIPanel uiPanel)
        {
            bool canAddPanel = _uiPanels.TryAdd(type, uiPanel);
            if (!canAddPanel)
                Utils.InfoPoint($"Can't add {type} ui panel");
        }

        private void SavePanel(string type, UIPanel uiPanel)
        {
            bool canAddPanel = _createdPanels.TryAdd(type, uiPanel);
            if (!canAddPanel)
                Utils.InfoPoint($"Can't save {type} ui panel");
        }

        private TPanel GetPanelPrefab<TPanel>(string type) where TPanel : UIPanel
        {
            _uiPanels.TryGetValue(type, out UIPanel panelPrefab);
            return (TPanel) panelPrefab;
        }

        private void LogPanelNotFound(string panelType)
        {
            string errorLog = $"Data of type ({panelType} not found!";
            Debug.LogError(errorLog);
        }
    }
}