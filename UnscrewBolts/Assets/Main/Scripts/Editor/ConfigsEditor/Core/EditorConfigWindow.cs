using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor.ConfigsEditor.Core
{
    public class EditorConfigWindow : OdinEditorWindow
    {
        private static EditorConfig _config = null;
        private static Action _onClose = null;

        [Header("Paths settings")]
        [SerializeField, FolderPath] private string _assetsPath = string.Empty;

        [Header("Colors settings")]
        [SerializeField] private Color _selectedColor = new Color(0.745f, 0.256f, 0.302f);

        [SerializeField] private Color _selectedInactiveColor = new Color(0.205f, 0.205f, 0.205f);

        public static void OpenWindow(EditorConfig config, Action onClose)
        {
            _config = config;
            _onClose = onClose;
            EditorConfigWindow window = CreateInstance<EditorConfigWindow>();
            float width = ConfigWindow.WIDTH;
            float height = ConfigWindow.HEIGHT;

            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(width, height);

            window.Show(true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(width, height);
        }

        protected override void OnImGUI()
        {
            if (_config == null)
            {
                Close();
                return;
            }

            if (_assetsPath == string.Empty)
                _assetsPath = _config.AssetsPath;

            base.OnImGUI();
        }

        [Button(Stretch = true, ButtonHeight = ConfigWindow.BUTTON_HEIGHT)]
        private void Save()
        {
            _config.SaveSettings(_assetsPath, _selectedColor, _selectedInactiveColor);

            _onClose.Invoke();
            Close();
        }

        [Title(ConfigWindow.SCRIPTS_BUTTONS)]
        [Button(Stretch = true, ButtonHeight = ConfigWindow.BUTTON_HEIGHT)]
        private void OpenCategoriesScript()
        {
            var categoriesScript = AssetDatabase.LoadMainAssetAtPath(Paths.CATEGORIES_PATH);
            AssetDatabase.OpenAsset(categoriesScript);
            Close();
        }

        [Button(Stretch = true, ButtonHeight = ConfigWindow.BUTTON_HEIGHT)]
        private void OpenConstantsScript()
        {
            var categoriesScript = AssetDatabase.LoadMainAssetAtPath(Paths.CONSTANTS_PATH);
            AssetDatabase.OpenAsset(categoriesScript);
            Close();
        }
    }
}