using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Scripts.Core.Utilities;
using Scripts.Data;
using Scripts.Data.Core;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Scripts.Editor.DataEditor
{
    public class DataEditor : OdinMenuEditorWindow
    {
        public static readonly Color SelectedColor = new Color(0.745f, 0.256f, 0.302f);
        public static readonly Color SelectedInactiveColor = new Color(0.205f, 0.205f, 0.205f);
        private static IDatabase _database;
        private static DataEditor _window;
        private static bool _isVisible;
        private static bool _isSetuped;

        public static void OpenEditorWindow()
        {
            _database = null;
            if (!_isSetuped)
            {
                _isSetuped = true;
                DataEditorMediator.OnDatabaseChanged += SetDatabase;
            }

            _window = GetWindow<DataEditor>();
            _window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            _window.Show();
        }

        public static void SetDatabase(IDatabase database)
        {
            _database = database;

            if (!_isVisible)
                return;

            var window = GetWindow<DataEditor>();
            if (window == null)
                Utils.Log("DataEditor", "No Opened window");
            else
            {
                window.OnBeginDrawEditors();
                window.BuildMenuTree();
                window.ForceMenuTreeRebuild();
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true);

            ValidateDatabase();

            SetupMenuStyle(tree);
            CreateDataTabs(tree);

            tree.SortMenuItemsByName();
            tree.EnumerateTree().SortMenuItemsByName();
            tree.EnumerateTree().AddThumbnailIcons();

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            bool isMenuNotSelected = MenuTree?.Selection == null;

            if (isMenuNotSelected)
                return;

            OdinMenuItem selected = MenuTree.Selection.FirstOrDefault();
            const int toolbarHeight = 23;
            bool selectedIsNull = selected == null;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

            // Draw label
            GUILayout.Label(selectedIsNull ? " " : selected.Name);

            DrawSaveButton();
            DrawLoadButton();
            DrawDeleteButton();

            SirenixEditorGUI.EndHorizontalToolbar();

            // LOCAL METHODS: -----------------------------

            void DrawSaveButton()
            {
                if (!SirenixEditorGUI.ToolbarButton("Save"))
                    return;

                _database.SaveData();
            }

            void DrawLoadButton()
            {
                if (!SirenixEditorGUI.ToolbarButton("Load"))
                    return;

                _database.ReloadData();
            }

            void DrawDeleteButton()
            {
                if (!SirenixEditorGUI.ToolbarButton("Delete"))
                    return;

                _database.DeleteData();
                _database.SaveData();
                _database.ReloadData();
            }
        }

        private void OnBecameVisible() =>
            _isVisible = true;

        private void OnBecameInvisible() =>
            _isVisible = false;

        private static void ValidateDatabase()
        {
            if (_database == null)
            {
                if (DataEditorMediator.Database == null)
                {
                    _database = new GameDatabase();
                    _database.Initialize();
                }
                else
                    _database = DataEditorMediator.Database;
            }
        }

        private static void SetupMenuStyle(OdinMenuTree tree)
        {
            tree.Config.DrawSearchToolbar = false;
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.DefaultMenuStyle.Height = 28;
            tree.DefaultMenuStyle.IndentAmount = 12;
            tree.DefaultMenuStyle.IconSize = 26;
            tree.DefaultMenuStyle.NotSelectedIconAlpha = 1;
            tree.DefaultMenuStyle.IconPadding = 4;
            tree.DefaultMenuStyle.SelectedColorDarkSkin = SelectedColor;
            tree.DefaultMenuStyle.SelectedInactiveColorDarkSkin = SelectedInactiveColor;
        }

        private static void CreateDataTabs(OdinMenuTree tree)
        {
            Dictionary<Type, GameData> allData = GetDataDictionary();

            foreach (GameData data in allData.Values)
            {
                string dataKey = data.DataKey;
                bool isKeyEmpty = string.IsNullOrEmpty(dataKey);

                if (isKeyEmpty)
                    continue;

                string className = GetNiceName(dataKey);
                tree.Add(className, data);
            }
        }

        private static Dictionary<Type, GameData> GetDataDictionary()
        {
            Dictionary<Type, GameData> datas = new Dictionary<Type, GameData>();
            IEnumerable<GameData> allData = _database.GetAllData();

            foreach (GameData data in allData)
            {
                Type type = data.GetType();
                datas.TryAdd(type, data);
            }

            return datas;
        }

        private static string GetNiceName(string text)
        {
            bool isTextNull = string.IsNullOrEmpty(text);

            if (isTextNull)
                return text;

            return Regex.Replace(text, "([a-z0-9])([A-Z0-9])", "$1 $2");
        }
    }
}