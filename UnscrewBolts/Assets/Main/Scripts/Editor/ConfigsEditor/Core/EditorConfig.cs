using System;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor.ConfigsEditor.Core
{
    [Serializable]
    public class EditorConfig : UnityEngine.ScriptableObject
    {
        [Header("Paths settings")]
        [SerializeField] private string _assetsPath = Paths.ASSETS_PATH;

        [Header("Colors settings")]
        [SerializeField] private Color _selectedColor = Colors.SelectedColor;

        [SerializeField] private Color _selectedInactiveColor = Colors.SelectedInactiveColor;

        public string AssetsPath => _assetsPath == string.Empty ? Paths.ASSETS_PATH : _assetsPath;
        public Color SelectedColor => _selectedColor;
        public Color SelectedInactiveColor => _selectedInactiveColor;

        public void SaveSettings()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void SaveSettings(string assetPath, Color selectedColor, Color selectedInactiveColor)
        {
            if (AssetDatabase.IsValidFolder(_assetsPath))
                _assetsPath = assetPath;

            _selectedColor = selectedColor;
            _selectedInactiveColor = selectedInactiveColor;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}