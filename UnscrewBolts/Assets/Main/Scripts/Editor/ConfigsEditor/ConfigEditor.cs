using System.IO;
using System.Linq;
using Scripts.Configs.Core;
using Scripts.Editor.ConfigsEditor.Core;
using Scripts.Editor.ConfigsEditor.ScriptableObject;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor.ConfigsEditor
{
    public class ConfigEditor : OdinMenuEditorWindow
    {
        private EditorConfig _config = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_config == null)
                GetSettings();
        }

        public static void OpenEditorWindow()
        {
            ConfigEditor window = GetWindow<ConfigEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true);

            SetupMenuStyle(tree);
            LoadAllEditorMeta(tree);

            tree.EnumerateTree().SortMenuItemsByName();
            tree.EnumerateTree().AddThumbnailIcons();

            return tree;
        }

        private void SetupMenuStyle(OdinMenuTree tree)
        {
            tree.Config.DrawSearchToolbar = true;
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.DefaultMenuStyle.Height = 28;
            tree.DefaultMenuStyle.IndentAmount = 12;
            tree.DefaultMenuStyle.IconSize = 24;
            tree.DefaultMenuStyle.NotSelectedIconAlpha = 1;
            tree.DefaultMenuStyle.IconPadding = 4;
            tree.DefaultMenuStyle.SelectedColorDarkSkin = _config.SelectedColor;
            tree.DefaultMenuStyle.SelectedInactiveColorDarkSkin = _config.SelectedInactiveColor;

            //tree.Add("Menu Style", tree.DefaultMenuStyle);
        }

        private void LoadAllEditorMeta(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath(menuPath: "",
                _config.AssetsPath,
                typeof(ScriptableConfig),
                includeSubDirectories: true);
        }

        protected override void OnBeginDrawEditors()
        {
            bool isMenuNotSelected = MenuTree?.Selection == null;

            if (isMenuNotSelected)
                return;

            OdinMenuTreeSelection selection = MenuTree.Selection;
            OdinMenuItem selected = MenuTree.Selection.FirstOrDefault();

            ScriptableConfig selectedConfig = selection.SelectedValue as ScriptableConfig;
            int toolbarHeight = MenuTree.Config.SearchToolbarHeight;
            bool selectedIsNull = selected == null;
            bool drawMetaMenuButtons = !selectedIsNull && selectedConfig;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

            // Draw label
            GUILayout.Label(selectedIsNull ? " " : selected.Name);

            // Create and Selects the newly created item in the editor
            DrawCreateButton(selectedConfig);

            if (drawMetaMenuButtons)
            {
                DrawDuplicateButton(selectedConfig);
                DrawSaveButton(selectedConfig);
                DrawDeleteButton(selectedConfig);
            }

            DrawConfigButton();

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DrawCreateButton(ScriptableConfig selectedConfig)
        {
            if (!SirenixEditorGUI.ToolbarButton("Create"))
                return;

            string metaPath = _config.AssetsPath;
            if (selectedConfig != null)
            {
                metaPath = AssetDatabase.GetAssetPath(selectedConfig);
                string fileName = Path.GetFileName(metaPath);
                metaPath = metaPath.Replace(fileName, "");
            }

            ScriptableObjectCreator.ShowDialog(selectedConfig, metaPath, SetupObject, true);
        }

        private void DrawDuplicateButton(ScriptableConfig selectedConfig)
        {
            if (!SirenixEditorGUI.ToolbarButton("Duplicate"))
                return;

            ScriptableObjectCreator.Duplicate(selectedConfig, null, SaveDuplicatedObject);
        }

        private void DrawSaveButton(ScriptableConfig selectedConfig)
        {
            if (!SirenixEditorGUI.ToolbarButton("Save"))
                return;

            string metaPath = AssetDatabase.GetAssetPath(selectedConfig);
            string newName = selectedConfig.FileName;

            if (string.IsNullOrWhiteSpace(newName))
                return;

            AssetDatabase.RenameAsset(metaPath, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            TrySelectMenuItemWithObject(selectedConfig);
        }

        private void DrawDeleteButton(ScriptableConfig selectedConfig)
        {
            if (!SirenixEditorGUI.ToolbarButton("Delete"))
                return;

            string metaPath = AssetDatabase.GetAssetPath(selectedConfig);

            AssetDatabase.DeleteAsset(metaPath);
            AssetDatabase.SaveAssets();
        }

        private void DrawConfigButton()
        {
            if (!SirenixEditorGUI.ToolbarButton(EditorIcons.SettingsCog))
                return;

            if (_config == null)
            {
                Debug.LogError("No meta editor config found!");
                return;
            }

            EditorConfigWindow.OpenWindow(_config, () =>
            {
                BuildMenuTree();
                ForceMenuTreeRebuild();
            });
            GUIUtility.ExitGUI();
        }

        private void SetupObject(ScriptableConfig obj)
        {
            obj.FileName = obj.name != ""
                ? obj.name
                : Titles.NEW_CONFIG_FILENAME;

            ScriptableObjectUtility.SaveAsset(obj);

            TrySelectMenuItemWithObject(obj);
            GUIUtility.ExitGUI();
        }

        private void SaveDuplicatedObject(ScriptableConfig obj)
        {
            obj.FileName += " (Clone)";
            ScriptableObjectUtility.SaveAsset(obj);
            TrySelectMenuItemWithObject(obj);
        }

        private static void AddDraggable<T>(OdinMenuTree tree) where T : ScriptableConfig
        {
            tree.EnumerateTree()
                .Where(odinMenuItem => odinMenuItem.Value as T)
                .ForEach(AddDragHandles);
        }

        private static void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += _ =>
                DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, allowMove: false, allowSwap: false);
        }

        private void GetSettings()
        {
            string path = Paths.EDITOR_CONFIG_PATH;
            string configName = Paths.EDITOR_CONFIG_NAME;

            EditorConfig obj = null;
            if (!AssetDatabase.IsValidFolder(path))
                obj = CreateEditorConfig(path + configName);
            else
            {
                obj = AssetDatabase.LoadAssetAtPath<EditorConfig>(path + configName);
                if (obj == null)
                    obj = CreateEditorConfig(path + configName);
            }

            _config = obj;
        }

        private static EditorConfig CreateEditorConfig(string path)
        {
            EditorConfig obj;
            AssetDatabase.CreateAsset(CreateInstance<EditorConfig>(), path);
            AssetDatabase.Refresh();
            obj = AssetDatabase.LoadAssetAtPath<EditorConfig>(path);
            return obj;
        }
    }
}